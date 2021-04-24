<template>
  <v-container id="user-profile" fluid tag="section">
    <v-row justify="center">
      <v-col cols="12" md="8">
        <material-card>
          <template v-slot:heading>
            <div class="text-h4 font-weight-light">Profile</div>
          </template>
          <v-form>
            <v-container class="py-0">
              <v-row>
                <v-col cols="12" md="12">
                  <v-text-field
                    v-if="currentUser.role === 'admin'"
                    :value="currentUser.userName"
                    label="Tên đăng nhập"
                    readonly
                  />
                  <v-text-field
                    v-else
                    :value="currentUser.userName"
                    label="MSSV"
                    readonly
                  />
                </v-col>
                <v-col cols="12" md="6">
                  <v-text-field
                    v-model="currentUser.name"
                    class="purple-input"
                    label="Họ và tên"
                  />
                </v-col>
                <v-col cols="12" md="6">
                  <v-text-field
                    label="Giới tính"
                    class="purple-input"
                    :value="gender"
                    readonly
                  />
                </v-col>
                <v-col cols="12" md="6">
                  <v-text-field
                    label="Số CMND"
                    v-model="currentUser.idNumber"
                    class="purple-input"
                  />
                </v-col>
                <v-col cols="12" md="6">
                  <v-text-field
                    label="Số điện thoại"
                    v-model="currentUser.phoneNumber"
                    class="purple-input"
                  />
                </v-col>
                <v-col cols="12" md="12">
                  <v-text-field
                    label="Địa chỉ"
                    v-model="currentUser.address"
                    class="purple-input"
                  />
                </v-col>
                <v-col cols="12" class="text-right">
                  <v-btn @click="$router.go(-1)" color="success" class="mr-0">
                    Trở về
                  </v-btn>
                </v-col>
              </v-row>
            </v-container>
          </v-form>
        </material-card>
      </v-col>

      <v-col cols="12" md="4">
        <material-card v-if="avatar" class="v-card-profile" :avatar="avatar">
          <v-col
            v-if="currentUser.role !== 'admin'"
            cols="12"
            class="text-center"
          >
            <v-dialog v-model="scheduleDialog" max-width="800px">
              <template v-slot:activator="{ on, attrs }">
                <v-btn color="success" v-bind="attrs" v-on="on">
                  {{ btnText }}
                </v-btn>
              </template>
              <v-card>
                <v-card-title>
                  <span class="headline"> {{ btnText }} </span>
                </v-card-title>
                <v-divider></v-divider>
                <v-card-text>
                  <v-container>
                    <v-data-table
                      :headers="headerS"
                      :items="currentSchedule"
                      :items-per-page="15"
                      class="elevation-1"
                      :footer-props="{
                        'items-per-page-text': 'Số cột mỗi trang',
                      }"
                    >
                      <template v-slot:[`item.genaralSchedule`]="{ item }">
                        <span>
                          {{ item.schedule }} {{ item.startTime }} -
                          {{ item.endTime }}, {{ item.roomName }}
                        </span>
                      </template>
                    </v-data-table>
                  </v-container>
                </v-card-text>
              </v-card>
            </v-dialog>
          </v-col>
          <v-card-text class="text-center">
            <h6 class="text-h4 md-1 grey--text">
              {{ currentUser.name }}
            </h6>

            <h4 class="text-h5 font-weight-light md-3 black--text">
              {{ role }}
            </h4>
          </v-card-text>
        </material-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import MaterialCard from "../../components/base/MaterialCard.vue";
import * as API from "../../constant/api";
import UserService from "../../services/user.service.js";
import StudentService from "../../services/student.service.js";
import TeacherService from "../../services/teacher.service.js";
import datetimeHelper from "../../helper/datetimeHelper.js";
export default {
  name: "Profile",
  props: ["userId"],
  data() {
    return {
      avatar: "",
      scheduleDialog: false,
      currentUser: {},
      currentSchedule: [],
      headerS: [
        {
          text: "Mã lớp",
          align: "start",
          sortable: false,
          value: "id",
        },
        {
          text: "Tên môn học",
          align: "start",
          sortable: false,
          value: "courseName",
        },
        {
          text: "Giáo viên",
          align: "start",
          sortable: false,
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
          sortable: false,
          value: "startDay",
        },
        {
          text: "Kết thúc",
          align: "start",
          sortable: false,
          value: "endDay",
        },
      ],
      items: [
        { name: "Nam", value: true },
        { name: "Nữ", value: false },
      ],
    };
  },
  components: {
    MaterialCard,
  },
  computed: {
    gender() {
      if (this.currentUser.gender) {
        return "Nam";
      }
      return "Nữ";
    },
    role() {
      if (this.currentUser.role === "admin") {
        return "Giáo Vụ";
      }
      if (this.currentUser.role === "teacher") {
        return "Giáo Viên";
      }
      return "Học Viên";
    },
    btnText() {
      if (this.currentUser.role === "teacher") {
        return "Lịch dạy";
      }
      if (this.currentUser.role === "student") {
        return "Lịch học";
      }
      return "";
    },
  },
  methods: {
    getProfile() {
      UserService.getUserById(this.userId).then(
        (response) => {
          this.currentUser = response.data;
          this.avatar = `${API.SERVER}/${this.currentUser.avatar}`;
          this.getSection();
        },
        (error) => {
          console.log(error);
        }
      );
    },
    hideDialog() {
      this.scheduleDialog = false;
    },
    getSection() {
      if (this.currentUser.role === "student") {
        StudentService.getStudentSchedule(this.currentUser.studentId).then(
          (response) => {
            this.currentSchedule = response.data;
            for (let i = 0; i < this.currentSchedule.length; i++) {
              //schedule
              let schedule = this.currentSchedule[i].schedule.split(",");
              this.currentSchedule[i].schedule = "";
              schedule.forEach((element) => {
                let num = parseInt(element) + 1;
                this.currentSchedule[i].schedule += "Thứ " + num + ", ";
              });
              // day
              this.currentSchedule[i].startDay = datetimeHelper.getDateFormat(
                this.currentSchedule[i].startDay
              );
              this.currentSchedule[i].endDay = datetimeHelper.getDateFormat(
                this.currentSchedule[i].endDay
              );
            }
          },
          (error) => {
            console.log(error);
          }
        );
      }
      if (this.currentUser.role === "teacher") {
        TeacherService.getTeacherSchedule(this.currentUser.teacherId).then(
          (response) => {
            this.currentSchedule = response.data;
            for (let i = 0; i < this.currentSchedule.length; i++) {
              //schedule
              let schedule = this.currentSchedule[i].schedule.split(",");
              this.currentSchedule[i].schedule = "";
              schedule.forEach((element) => {
                let num = parseInt(element) + 1;
                this.currentSchedule[i].schedule += "Thứ " + num + ", ";
              });
              // day
              this.currentSchedule[i].startDay = datetimeHelper.getDateFormat(
                this.currentSchedule[i].startDay
              );
              this.currentSchedule[i].endDay = datetimeHelper.getDateFormat(
                this.currentSchedule[i].endDay
              );
            }
          },
          (error) => {
            console.log(error);
          }
        );
      }
    },
  },
  mounted() {
    this.getProfile();
  },
};
</script>