<template>
  <v-container id="section-register" fluid tag="section">
    <v-row justify="center">
      <v-col cols="12">
        <material-card>
          <template v-slot:heading>
            <div class="text-h4 font-weight-light">Đăng ký học</div>
          </template>
          <template v-if="registrationTime == false">
            <div
              style="height: 400px"
              class="text-h5 font-weight-bold text-center"
            >
              Không phải thời gian đăng ký học phần!
            </div>
          </template>
          <template v-else>
            <v-data-table
              :headers="headersS"
              :items="registedSections"
              class="elevation-1"
              :loading="isLoading"
              loading-text="Đang tải dữ liệu..."
              :hide-default-footer="true"
              :disable-pagination="true"
            >
              <template v-slot:top>
                <div class="font-weight-bold ml-3">
                  Danh sách các học phần đã đăng ký
                </div>
              </template>
              <template v-slot:[`item.genaralSchedule`]="{ item }">
                <span>
                  {{ item.schedule }} {{ item.startTime }} - {{ item.endTime }},
                  {{ item.roomName }}
                </span>
              </template>
              <template v-slot:[`item.slot`]="{ item }">
                <span> {{ item.slot }}/{{ item.maxSlot }} </span>
              </template>
              <template v-slot:[`item.actions`]="{ item }">
                <v-icon
                  color="red"
                  title="Xóa đăng ký"
                  small
                  class="mr-2"
                  @click="deleteRegistedSection(item)"
                  >mdi-delete</v-icon
                >
              </template>
            </v-data-table>
            <!-- sections for register -->
            <v-divider class="my-10"></v-divider>
            <v-data-table
              :headers="headers"
              :items="sectionListforRegistration"
              class="elevation-1"
              :loading="isLoading"
              loading-text="Đang tải dữ liệu..."
              :hide-default-footer="false"
              :footer-props="{
                'items-per-page-text': 'Số hàng mỗi trang',
              }"
            >
              <template v-slot:top>
                <div class="font-weight-bold ml-3">
                  Danh sách học phần có thể đăng ký
                </div>
              </template>
              <template v-slot:[`item.genaralSchedule`]="{ item }">
                <span>
                  {{ item.schedule }} {{ item.startTime }} - {{ item.endTime }},
                  {{ item.roomName }}
                </span>
              </template>
              <template v-slot:[`item.slot`]="{ item }">
                <span> {{ item.slot }}/{{ item.maxSlot }} </span>
              </template>
              <template v-slot:[`item.actions`]="{ item }">
                <v-icon
                  color="primary"
                  title="Đăng ký"
                  small
                  class="mr-2"
                  @click="register(item)"
                  >mdi-plus-circle</v-icon
                >
              </template>
            </v-data-table>
          </template>
        </material-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import MaterialCard from "../../../components/base/MaterialCard.vue";
import datetimeHelper from "../../../helper/datetimeHelper";
import registrationTimeService from "../../../services/registration-time.service";
import sectionRegisterService from "../../../services/section-register.service";
export default {
  name: "sectionRegister",
  components: {
    MaterialCard,
  },
  data() {
    return {
      registrationTime: true,
      registedSections: [],
      sectionListforRegistration: [],
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
        {
          text: "Số lượng",
          value: "slot",
          sortable: false,
          width: 90,
          align: "center",
        },
        {
          text: "",
          width: 100,
          value: "actions",
          sortable: false,
          align: "right",
        },
      ],
      headersS: [
        {
          text: "Mã lớp",
          align: "start",
          sortable: true,
          value: "sectionId",
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
        {
          text: "Số lượng",
          value: "slot",
          sortable: false,
          width: 90,
          align: "center",
        },
        {
          text: "",
          width: 100,
          value: "actions",
          sortable: false,
          align: "right",
        },
      ],
      isLoading: false,
    };
  },
  computed: {
    currentUser() {
      return this.$store.state.auth.user.userInfo;
    },
  },
  methods: {
    getRegistrationTime() {
      registrationTimeService.getRegistrationTime().then(
        (response) => {
          //console.log(response.data);
          //this.registrationTime = response.data;
          let start = new Date(response.data.startDateTime);
          let end = new Date(response.data.endDateTime);
          let now = new Date();
          if (now >= start && now <= end) {
            this.registrationTime = true;
          } else {
            this.registrationTime = false;
          }
        },
        (err) => {
          console.log(err);
        }
      );
    },
    getRegistedSections() {
      this.isLoading = false;
      sectionRegisterService
        .getRegisterListByStudentId(this.currentUser.studentId)
        .then(
          (response) => {
            this.registedSections = response.data;
            for (let i = 0; i < this.registedSections.length; i++) {
              let schedule = this.registedSections[i].schedule.split(",");
              this.registedSections[i].schedule = "";
              schedule.forEach((element) => {
                let num = parseInt(element) + 1;
                this.registedSections[i].schedule += "Thứ " + num + ", ";
              });
              this.registedSections[i].startDay = datetimeHelper.getDateFormat(
                this.registedSections[i].startDay
              );
              this.registedSections[i].endDay = datetimeHelper.getDateFormat(
                this.registedSections[i].endDay
              );
            }
            this.isLoading = false;
          },
          (err) => {
            console.log(err);
          }
        );
    },
    getSectionListforRegistration() {
      this.isLoading = false;
      sectionRegisterService
        .getSectionForRegisterByStudentId(this.currentUser.studentId)
        .then(
          (response) => {
            this.sectionListforRegistration = response.data;
            for (let i = 0; i < this.sectionListforRegistration.length; i++) {
              let schedule = this.sectionListforRegistration[i].schedule.split(
                ","
              );
              this.sectionListforRegistration[i].schedule = "";
              schedule.forEach((element) => {
                let num = parseInt(element) + 1;
                this.sectionListforRegistration[i].schedule +=
                  "Thứ " + num + ", ";
              });
              this.sectionListforRegistration[
                i
              ].startDay = datetimeHelper.getDateFormat(
                this.sectionListforRegistration[i].startDay
              );
              this.sectionListforRegistration[
                i
              ].endDay = datetimeHelper.getDateFormat(
                this.sectionListforRegistration[i].endDay
              );
            }
            this.isLoading = false;
          },
          (err) => {
            console.log(err);
          }
        );
    },
    deleteRegistedSection(item) {
      console.log(item);
      sectionRegisterService
        .deleteStudentRegistration(this.currentUser.studentId, item.sectionId)
        .then(
          () => {
            this.$toast("Xóa đăng ký học phần thành công!", {
              color: "success",
              x: "right",
              y: "top",
              showClose: true,
              closeIcon: "mdi-close",
            });
            this.getRegistedSections();
            this.getSectionListforRegistration();
          },
          (err) => {
            console.log(err);
          }
        );
    },
    register(item) {
      sectionRegisterService
        .registerSection(this.currentUser.studentId, item.id)
        .then(
          () => {
            this.newUserDialog = false;
            this.$toast("Đăng ký học phần thành công!", {
              color: "success",
              x: "right",
              y: "top",
              showClose: true,
              closeIcon: "mdi-close",
            });
            this.getRegistedSections();
            this.getSectionListforRegistration();
          },
          (err) => {
            this.$toast(`${err.response.data.error}`, {
              color: "error",
              x: "right",
              y: "top",
              showClose: true,
              closeIcon: "mdi-close",
            });
          }
        );
    },
  },
  mounted() {
    this.getRegistrationTime();
    this.getRegistedSections();
    this.getSectionListforRegistration();
  },
};
</script>
        