<template>
  <v-container id="schedule" fluid tag="section">
    <v-row justify="center">
      <v-col cols="12">
        <material-card>
          <template v-slot:heading>
            <div class="text-h4 font-weight-light">Danh sách lớp</div>
          </template>
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
                      >
                      </v-data-table>
                    </v-col>
                  </v-row>
                </v-container>
              </v-card-text>
            </v-card>
          </v-dialog>
          <v-data-table
            :headers="headers"
            :items="sections"
            class="elevation-1"
            :loading="isLoading"
            loading-text="Đang tải dữ liệu..."
            :hide-default-footer="true"
          >
            <template v-slot:top>
              <v-row class="p-3">
                <v-col cols="12" sm="6" md="4">
                  <v-select
                    v-model="sectionFilter"
                    :items="sectionFilterItems"
                    label="Hiển thị học phần"
                    dense
                    @change="filterSection"
                  >
                  </v-select>
                </v-col>
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
            </template>
          </v-data-table>
        </material-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import MaterialCard from "../../../components/base/MaterialCard.vue";
import datetimeHelper from "../../../helper/datetimeHelper";
import SectionService from "../../../services/section.service";
import TeacherService from "../../../services/teacher.service";
export default {
  name: "schedule",
  components: {
    MaterialCard,
  },
  data() {
    return {
      isLoading: false,
      selectedSection: null,
      viewStudentsDialog: false,
      students: [],
      headersS: [
        { text: "MSSV", value: "userName" },
        { text: "Họ tên", value: "name", width: 100 },
        { text: "Giới tính", value: "gender", width: 100 },
        { text: "CMND", value: "idNumber" },
        { text: "SĐT", value: "phoneNumber" },
        { text: "Địa chỉ", value: "address" },
      ],
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
        {
          text: "Số lượng",
          value: "slot",
          sortable: false,
          width: 90,
          align: "center",
        },
        { text: "", width: 100, value: "actions", sortable: false },
      ],
      sections: [],
      oriSection: [],
      sectionFilter: "all",
      sectionFilterItems: [
        { text: "Tất cả", value: "all" },
        { text: "Hiện tại", value: "now" },
      ],
    };
  },
  computed: {},
  methods: {
    getSections() {
      this.isLoading = true;
      TeacherService.getMySection().then(
        (response) => {
          this.sections = response.data;
          console.log(this.sections);
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
          this.oriSection = this.sections;
        },
        (err) => {
          console.log(err);
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
        (err) => {
          console.log(err);
        }
      );
    },
    viewStudentList(item) {
      this.selectedSection = item;
      this.getStudentList();
      this.viewStudentsDialog = true;
    },
    filterSection() {
      if (this.sectionFilter == "now") {
        this.sections = this.oriSection.filter((section) => {
          let dateNow = new Date();
          let endDay = new Date(section.endDay.split("-").reverse().join("-"));
          return dateNow <= endDay;
        });
      } else {
        this.sections = this.oriSection;
      }
    },
  },
  mounted() {
    this.getSections();
  },
};
</script>