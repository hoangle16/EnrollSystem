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
                <v-dialog v-model="newSectionDialog" max-width="700px">
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
                <!--delete confirm dialog -->
                <v-dialog persistent v-model="deleteDialog" max-width="500px">
                  <v-card>
                    <v-card-title class="headline"
                      >Bạn có chắc muốn xóa học phần này</v-card-title
                    >
                    <v-card-actions>
                      <v-spacer></v-spacer>
                      <v-btn color="success" @click="confirmDelete()"
                        >Xóa</v-btn
                      >
                      <v-btn color="error" @click="deleteDialog = false">
                        Hủy</v-btn
                      >
                      <v-spacer></v-spacer>
                    </v-card-actions>
                  </v-card>
                </v-dialog>
                <v-dialog v-model="viewStudentsDialog" max-width="800px">
                  <v-card>
                    <v-card-title>
                      <span class="headline">Danh sách học viên</span>
                    </v-card-title>
                    <v-divider></v-divider>
                    <v-card-text>
                      <v-container>
                        <v-row>
                          <v-col cols="12">
                            <v-data-table
                              :headers="headersS"
                              :items="students"
                              :hide-default-footer="true"
                              class="elevation-1"
                              :footer-props="{
                                'items-per-page-text': 'Số hàng mỗi trang',
                              }"
                            >
                            </v-data-table>
                          </v-col>
                        </v-row>
                      </v-container>
                    </v-card-text>
                  </v-card>
                </v-dialog>
                <!-- Edit section -->
                <v-dialog v-model="editSectionDialog" max-width="700px">
                  <v-card>
                    <v-card-title>
                      <span class="headline">Cập nhật học phần</span>
                    </v-card-title>
                    <v-divider></v-divider>
                    <v-card-text>
                      <v-container>
                        <v-row>
                          <v-col cols="12">
                            <v-autocomplete
                              dense
                              clearable
                              v-model="selectedSection.courseId"
                              :items="courseItems"
                              label="Tên môn học"
                            ></v-autocomplete>
                          </v-col>
                          <v-col cols="12" md="6">
                            <v-menu
                              v-model="startDayMenu1"
                              :close-on-content-click="false"
                              :nudge-right="40"
                              transition="scale-transition"
                              offset-y
                              min-width="auto"
                            >
                              <template v-slot:activator="{ on, attrs }">
                                <v-text-field
                                  v-model="selectedSection.startDay"
                                  label="Ngày bắt đầu"
                                  readonly
                                  v-bind="attrs"
                                  v-on="on"
                                ></v-text-field>
                              </template>
                              <v-date-picker
                                v-model="selectedSection.startDay"
                                @input="startDayMenu1 = false"
                                :min="dateNow"
                                @change="scheduleChangeEdit"
                              ></v-date-picker>
                            </v-menu>
                          </v-col>
                          <v-col cols="12" md="6">
                            <v-menu
                              v-model="endDayMenu1"
                              :close-on-content-click="false"
                              :nudge-right="40"
                              transition="scale-transition"
                              offset-y
                              min-width="auto"
                            >
                              <template v-slot:activator="{ on, attrs }">
                                <v-text-field
                                  v-model="selectedSection.endDay"
                                  label="Ngày kết thúc"
                                  readonly
                                  v-bind="attrs"
                                  v-on="on"
                                ></v-text-field>
                              </template>
                              <v-date-picker
                                v-model="selectedSection.endDay"
                                @input="endDayMenu1 = false"
                                @change="scheduleChangeEdit"
                                :min="dateNow"
                              ></v-date-picker>
                            </v-menu>
                          </v-col>
                          <v-col cols="12" md="12">
                            <v-select
                              v-model="selectedSection.schedule"
                              :items="scheduleItems"
                              label="Ngày học"
                              dense
                              chips
                              multiple
                              clearable
                              @change="scheduleChangeEdit"
                            ></v-select>
                          </v-col>
                          <v-col cols="12" md="6">
                            <v-select
                              v-model="selectedSection.startTime"
                              :items="timeItems"
                              label="Thời gian bắt đầu"
                              clearable
                              dense
                              @change="scheduleChangeEdit"
                            >
                            </v-select>
                          </v-col>
                          <v-col cols="12" md="6">
                            <v-select
                              v-model="selectedSection.endTime"
                              :items="timeItems"
                              label="Thời gian kết thúc"
                              clearable
                              :item-disabled="checkIsItemDisabled"
                              dense
                              @change="scheduleChangeEdit"
                            >
                            </v-select>
                          </v-col>
                          <v-col cols="12" md="6">
                            <v-select
                              v-model="selectedSection.teacherId"
                              :items="teacherItems"
                              label="Giáo viên"
                              clearable
                              dense
                            ></v-select>
                          </v-col>
                          <v-col cols="12" md="6">
                            <v-select
                              v-model="selectedSection.roomId"
                              :items="roomItems"
                              label="Phòng học"
                              dense
                              clearable
                            ></v-select>
                          </v-col>
                          <v-col cols="12">
                            <v-text-field
                              v-model="selectedSection.maxSlot"
                              label="Số lượng"
                              type="number"
                            ></v-text-field>
                          </v-col>
                        </v-row>
                      </v-container>
                    </v-card-text>
                    <v-card-actions>
                      <v-spacer></v-spacer>
                      <v-btn color="success" @click="confirmEditSection"
                        >Cập nhật</v-btn
                      >
                      <v-btn color="error" @click="editSectionDialog = false"
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
      headersS: [
        { text: "MSSV", value: "userName" },
        { text: "Họ tên", value: "name", width: 100 },
        { text: "Giới tính", value: "gender", width: 100 },
        { text: "CMND", value: "idNumber" },
        { text: "SĐT", value: "phoneNumber" },
        { text: "Địa chỉ", value: "address" },
      ],
      students: [],
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
      viewStudentsDialog: false,
      sections: [],
      newSection: {},
      selectedSection: {},
      search: "",
      isLoading: false,
      newSectionDialog: false,
      editSectionDialog: false,
      courseItems: [],
      teacherItems: [],
      roomItems: [],
      startDayMenu: false,
      endDayMenu: false,
      startDayMenu1: false,
      endDayMenu1: false,
      deleteDialog: false,
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

    getTeacherAvailable(data) {
      TeacherService.getTeachersAvailable(data).then(
        (response) => {
          this.teacherItems = [];
          response.data.forEach((el) => {
            this.teacherItems.push({ text: el.name, value: el.teacherId });
          });
        },
        (error) => {
          console.log(error);
        }
      );
    },
    getRoomAvailable(data) {
      RoomService.getEmptyRoom(data).then(
        (response) => {
          this.roomItems = [];
          response.data.forEach((el) => {
            this.roomItems.push({ text: el.name, value: el.id });
          });
        },
        (error) => {
          console.log(error);
        }
      );
    },
    getStudentList() {
      SectionService.getSectionById(this.selectedSection.id).then(
        (response) => {
          this.students = response.data.students;
          this.students.forEach((el) => {
            el.gender = el.gender ? "Nam" : "Nữ";
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
        this.getTeacherAvailable(this.newSection);
        this.getRoomAvailable(this.newSection);
      }
    },
    scheduleChangeEdit() {
      if (
        this.selectedSection.startDay != null &&
        this.selectedSection.endDay != null &&
        this.selectedSection.schedule != null &&
        this.selectedSection.startTime != null &&
        this.selectedSection.endTime != null
      ) {
        this.getTeacherAvailable(this.selectedSection);
        this.getRoomAvailable(this.selectedSection);
      }
    },
    createSection() {
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
      this.selectedSection = { ...item };
      this.selectedSection.startDay = this.selectedSection.startDay
        .split("-")
        .reverse()
        .join("-");
      this.selectedSection.endDay = this.selectedSection.endDay
        .split("-")
        .reverse()
        .join("-");
      this.selectedSection.schedule = this.selectedSection.schedule
        .replace(/[^0-9,-]/g, "")
        .split(",")
        .map((x) => --x);
      this.selectedSection.schedule.splice(
        this.selectedSection.schedule.length - 1,
        1
      );
      this.teacherItems.push({
        text: this.selectedSection.teacherName,
        value: this.selectedSection.teacherId,
      });
      this.roomItems.push({
        text: this.selectedSection.roomName,
        value: this.selectedSection.roomId,
      });
      this.editSectionDialog = true;
    },
    confirmEditSection() {
      if (this.selectedSection != null) {
        SectionService.editSection(
          this.selectedSection.id,
          this.selectedSection
        ).then(
          (response) => {
            console.log(response.data);
            this.$toast("Cập nhật học phần thành công!", {
              color: "success",
              x: "right",
              y: "top",
              showClose: true,
              closeIcon: "mdi-close",
            });
            this.editSectionDialog = false;
            this.getSectionList();
          },
          (error) => {
            console.log(error);
          }
        );
      }
    },
    deleteSection(item) {
      this.selectedSection = item;
      this.deleteDialog = true;
    },
    confirmDelete() {
      if (this.selectedSection != null) {
        SectionService.deleteSection(this.selectedSection.id).then(
          (response) => {
            console.log(response.data);
            this.$toast("Xóa học phần thành công", {
              color: "success",
              x: "right",
              y: "top",
              showClose: true,
              closeIcon: "mdi-close",
            });
            this.deleteDialog = false;
            this.getSectionList();
          },
          (error) => {
            console.log(error);
          }
        );
      }
    },
    viewStudentList(item) {
      this.selectedSection = item;
      this.getStudentList();
      this.viewStudentsDialog = true;
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