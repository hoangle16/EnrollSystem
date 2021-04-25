<template>
  <v-container id="section-manager" fluid tab="section">
    <v-row justify="center">
      <v-col cols="12">
        <meterial-card>
          <template v-slot:heading>
            <div class="text-h4 font-weight-light">Quản lý học phần</div>
          </template>
          <v-data-table
            :headers="headers"
            :items="sections"
            :search="search"
            :items-per-page="15"
            class="elevation-1"
            :loading="isLoading"
            loading-text="Đang tải dữ liệu..."
            :footer-props="{
              'items-per-page-text': 'Số hàng mỗi trang',
            }"
          >
            <template v-slot:top>
              <v-row class="p-3">
                <v-col cols="12" sm="6" md="4">
                  <v-text-field
                    dense
                    v-model="search"
                    append-icon="mdi-magnify"
                    label="Tìm kiếm"
                    hide-details
                  ></v-text-field>
                </v-col>
                <v-spacer></v-spacer>
                <v-dialog v-model="newSectionDialog" max-width="600px">
                  <template v-slot:activator="{ on, attrs }">
                    <v-btn
                      small
                      color="success"
                      dark
                      class="mb-2"
                      v-bind="attrs"
                      v-on="on"
                    >
                      <v-icon dark class="mr-2">mdi-plus-circle-outline</v-icon>
                      Thêm
                    </v-btn>
                  </template>
                  <v-card>
                    <v-card-title>
                      <span class="headline"> Học phần mới </span>
                    </v-card-title>
                    <v-divider></v-divider>
                    <v-card-text>
                      <v-container>
                        <v-row>
                          <v-col cols="12">
                            <v-autocomplete
                              dense
                              clearable
                              v-model="newSection.courseId"
                              :items="courseItems"
                              label="Tên môn học"
                            ></v-autocomplete>
                          </v-col>
                          <v-col cols="12" md="6">
                            <v-menu
                              v-model="startDayMenu"
                              :close-on-content-click="false"
                              :nudge-right="40"
                              transition="scale-transition"
                              offset-y
                              min-width="auto"
                            >
                              <template v-slot:activator="{ on, attrs }">
                                <v-text-field
                                  v-model="newSection.startDay"
                                  label="Ngày bắt đầu"
                                  readonly
                                  v-bind="attrs"
                                  v-on="on"
                                ></v-text-field>
                              </template>
                              <v-date-picker
                                v-model="newSection.startDay"
                                @input="startDayMenu = false"
                                :min="dateNow"
                                @change="scheduleChange"
                              ></v-date-picker>
                            </v-menu>
                          </v-col>
                          <v-col cols="12" md="6">
                            <v-menu
                              v-model="endDayMenu"
                              :close-on-content-click="false"
                              :nudge-right="40"
                              transition="scale-transition"
                              offset-y
                              min-width="auto"
                            >
                              <template v-slot:activator="{ on, attrs }">
                                <v-text-field
                                  v-model="newSection.endDay"
                                  label="Ngày kết thúc"
                                  readonly
                                  v-bind="attrs"
                                  v-on="on"
                                ></v-text-field>
                              </template>
                              <v-date-picker
                                v-model="newSection.endDay"
                                @input="endDayMenu = false"
                                :min="newSection.startDay"
                                @change="scheduleChange"
                              ></v-date-picker>
                            </v-menu>
                          </v-col>
                          <v-col cols="12" md="12">
                            <v-select
                              v-model="newSection.schedule"
                              :items="scheduleItems"
                              label="Ngày học"
                              dense
                              chips
                              multiple
                              clearable
                              @change="scheduleChange"
                            ></v-select>
                          </v-col>
                          <v-col cols="12" md="6">
                            <v-select
                              v-model="newSection.startTime"
                              :items="timeItems"
                              label="Thời gian bắt đầu"
                              clearable
                              dense
                              @change="scheduleChange"
                            >
                            </v-select>
                          </v-col>
                          <v-col cols="12" md="6">
                            <v-select
                              v-model="newSection.endTime"
                              :items="timeItems"
                              label="Thời gian kết thúc"
                              clearable
                              :item-disabled="checkIsItemDisabled"
                              dense
                              @change="scheduleChange"
                            >
                            </v-select>
                          </v-col>
                          <v-col cols="12" md="6">
                            <v-select
                              v-model="newSection.teacherId"
                              :items="teacherItems"
                              label="Giáo viên"
                              clearable
                              dense
                            ></v-select>
                          </v-col>
                          <v-col cols="12" md="6">
                            <v-select
                              v-model="newSection.roomId"
                              :items="roomItems"
                              label="Phòng học"
                              dense
                              clearable
                            ></v-select>
                          </v-col>
                          <v-col cols="12">
                            <v-text-field
                              v-model="newSection.maxSlot"
                              label="Số lượng"
                              type="number"
                            ></v-text-field>
                          </v-col>
                        </v-row>
                      </v-container>
                    </v-card-text>
                    <v-card-actions>
                      <v-spacer></v-spacer>
                      <v-btn color="success" @click="createSection">Thêm</v-btn>
                      <v-btn color="error" @click="newSectionDialog = false"
                        >Hủy</v-btn
                      >
                    </v-card-actions>
                  </v-card>
                </v-dialog>
              </v-row>
            </template>
            <template v-slot:[`item.genaralSchedule`]="{ item }">
              <span>
                {{ item.schedule }} {{ item.startTime }} - {{ item.endTime }},
                {{ item.roomName }}
              </span>
            </template>
            <template v-slot:[`item.actions`]="{ item }">
              <v-icon
                color="blue lighten-2"
                title="Danh sách học viên"
                small
                class="mr-2"
                @click="viewStudentList(item)"
                >mdi-text-account</v-icon
              >
              <v-icon
                title="Chỉnh sửa"
                small
                color="primary"
                class="mr-2"
                @click="editSection(item)"
                >mdi-pencil</v-icon
              >
              <v-icon color="red" title="Xóa" small @click="deleteSection(item)"
                >mdi-delete</v-icon
              >
            </template>
          </v-data-table>
        </meterial-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import MeterialCard from "../../../components/base/MaterialCard.vue";
import datetimeHelper from "../../../helper/datetimeHelper";
import SectionService from "../../../services/section.service.js";
import CourseService from "../../../services/course.service.js";
import TeacherService from "../../../services/teacher.service.js";
import RoomService from "../../../services/room.service.js";
export default {
  name: "SectionsManager",
  components: {
    MeterialCard,
  },
  data() {
    return {
      headers: [
        {
          text: "Mã lớp",
          align: "start",
          sortable: true,
          value: "id",
        },
        {
          text: "Tên môn học",
          align: "start",
          sortable: true,
          value: "courseName",
        },
        {
          text: "Giáo viên",
          align: "start",
          sortable: true,
          value: "teacherName",
        },
        {
          text: "Thời khóa biểu",
          align: "start",
          sortable: false,
          value: "genaralSchedule",
        },
        {
          text: "Bắt đầu",
          align: "start",
          sortable: true,
          value: "startDay",
        },
        {
          text: "Kết thúc",
          align: "start",
          sortable: true,
          value: "endDay",
        },
        { text: "", width: 100, value: "actions", sortable: false },
      ],
      sections: [],
      newSection: {},
      selectedSection: {},
      search: "",
      isLoading: false,
      newSectionDialog: false,
      courseItems: [],
      teacherItems: [],
      roomItems: [],
      startDayMenu: false,
      endDayMenu: false,
      scheduleItems: [
        { text: "Thứ 2", value: 1 },
        { text: "Thứ 3", value: 2 },
        { text: "Thứ 4", value: 3 },
        { text: "Thứ 5", value: 4 },
        { text: "Thứ 6", value: 5 },
        { text: "Thứ 7", value: 6 },
      ],
      timeItems: [
        { text: "Tiết 1", value: 1 },
        { text: "Tiết 2", value: 2 },
        { text: "Tiết 3", value: 3 },
        { text: "Tiết 4", value: 4 },
        { text: "Tiết 5", value: 5 },
        { text: "Tiết 6", value: 6 },
        { text: "Tiết 7", value: 7 },
        { text: "Tiết 8", value: 8 },
        { text: "Tiết 9", value: 9 },
        { text: "Tiết 10", value: 10 },
      ],
    };
  },
  computed: {
    dateNow() {
      return new Date().toISOString().substr(0, 10);
    },
  },
  methods: {
    getSectionList() {
      this.isLoading = true;
      SectionService.getSections().then(
        (response) => {
          this.sections = response.data;
          for (let i = 0; i < this.sections.length; i++) {
            let schedule = this.sections[i].schedule.split(",");
            this.sections[i].schedule = "";
            schedule.forEach((element) => {
              let num = parseInt(element) + 1;
              this.sections[i].schedule += "Thứ " + num + ", ";
            });
            this.sections[i].startDay = datetimeHelper.getDateFormat(
              this.sections[i].startDay
            );
            this.sections[i].endDay = datetimeHelper.getDateFormat(
              this.sections[i].endDay
            );
          }
          this.isLoading = false;
          console.log(this.sections);
        },
        (error) => {
          console.log(error);
          this.isLoading = false;
        }
      );
    },
    getCourses() {
      CourseService.getCourse().then(
        (response) => {
          response.data.forEach((el) => {
            this.courseItems.push({ text: el.name, value: el.id });
          });
        },
        (error) => {
          console.log(error);
        }
      );
    },

    getTeacherAvailable() {
      TeacherService.getTeachersAvailable(this.newSection).then(
        (response) => {
          response.data.forEach((el) => {
            this.teacherItems.push({ text: el.name, value: el.teacherId });
          });
        },
        (error) => {
          console.log(error);
        }
      );
    },
    getRoomAvailable() {
      RoomService.getEmptyRoom(this.newSection).then(
        (response) => {
          response.data.forEach((el) => {
            this.roomItems.push({ text: el.name, value: el.id });
          });
        },
        (error) => {
          console.log(error);
        }
      );
    },
    scheduleChange() {
      if (
        this.newSection.startDay != null &&
        this.newSection.endDay != null &&
        this.newSection.schedule != null &&
        this.newSection.startTime != null &&
        this.newSection.endTime != null
      ) {
        this.getTeacherAvailable();
        this.getRoomAvailable();
      }
    },
    createSection() {
      console.log(this.newSection);
      SectionService.createSection(this.newSection).then(
        (response) => {
          console.log(response.data);
          this.$toast("Tạo học phần mới thành công!", {
            color: "success",
            x: "right",
            y: "top",
            showClose: true,
            closeIcon: "mdi-close",
          });
          this.newSectionDialog = false;
          this.getSectionList();
        },
        (error) => {
          console.log(error);
        }
      );
    },
    editSection(item) {
      console.log(item);
    },
    deleteSection(item) {
      console.log(item);
    },
    viewStudentList(item) {
      console.log(item);
    },
    checkIsItemDisabled(item) {
      return item.value < this.newSection.startTime;
    },
  },
  mounted() {
    this.getSectionList();
    this.getCourses();
  },
};
</script>