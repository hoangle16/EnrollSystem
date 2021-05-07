<template>
  <v-container id="section-list" fluid tag="section">
    <v-row justify="center">
      <v-col cols="12">
        <material-card>
          <template v-slot:heading>
            <div class="text-h4 font-weight-light">Lịch học</div>
          </template>
          <v-dialog v-model="viewStudentsDialog" max-width="800px">
            <v-card>
              <v-card-title>
                <span class="headline">Danh sách lớp</span>
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
import sectionService from "../../../services/section.service";
import studentService from "../../../services/student.service";
export default {
  name: "sectionList",
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
        { text: "Họ tên", value: "name" },
        { text: "Giới tính", value: "gender", width: 100 },
        { text: "SĐT", value: "phoneNumber" }
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
    };
  },
  computed: {
    currentUser() {
      return this.$store.state.auth.user.userInfo;
    },
  },
  methods: {
    getSections() {
      console.log(this.currentUser);
      this.isLoading = true;
      studentService.getStudentSchedule(this.currentUser.studentId).then(
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
        (err) => {
          console.log(err);
        }
      );
    },
    getStudentList() {
      sectionService.getSectionById(this.selectedSection.id).then(
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
  },
  mounted() {
    this.getSections();
  },
};
</script>