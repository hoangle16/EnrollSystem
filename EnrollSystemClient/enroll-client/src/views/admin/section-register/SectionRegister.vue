<template>
  <v-container id="section-register" fluid tab="section">
    <v-row justify="center">
      <v-col cols="12">
        <material-card>
          <template v-slot:heading>
            <div class="text-h4 font-weight-light">
              Quản lý đăng ký học phần
            </div>
          </template>
          <v-data-table
            :headers="headers"
            :items="registerItem"
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
                <v-spacer></v-spacer>
                <v-dialog v-model="registrationTimeDialog" max-width="500px">
                  <template v-slot:activator="{ on, attrs }">
                    <v-btn
                      small
                      color="success"
                      dark
                      class="mb-2"
                      v-bind="attrs"
                      v-on="on"
                      title="Thiết lặp thời gian đăng ký"
                    >
                      <v-icon dark class="mr-2">mdi-timetable</v-icon>
                      Thời gian đăng ký
                    </v-btn>
                  </template>
                  <v-card>
                    <v-card-title>
                      <span class="headline">
                        Thiết lặp thời gian đăng ký học phần
                      </span>
                    </v-card-title>
                    <v-divider></v-divider>
                    <v-card-text>
                      <v-container>
                        <v-row>
                          <v-col cols="12">
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
                                  v-model="registrationTime.startDateTime"
                                  label="Ngày bắt đầu"
                                  readonly
                                  v-bind="attrs"
                                  v-on="on"
                                ></v-text-field>
                              </template>
                              <v-date-picker
                                v-model="registrationTime.startDateTime"
                                @input="startDayMenu = false"
                                :min="new Date().toISOString().substr(0, 10)"
                                 locale="vi-vn"
                              ></v-date-picker>
                            </v-menu>
                          </v-col>
                          <v-col cols="12">
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
                                  v-model="registrationTime.endDateTime"
                                  label="Ngày bắt đầu"
                                  readonly
                                  v-bind="attrs"
                                  v-on="on"
                                ></v-text-field>
                              </template>
                              <v-date-picker
                                v-model="registrationTime.endDateTime"
                                @input="endDayMenu = false"
                                :min="minDate"
                                 locale="vi-vn"
                              ></v-date-picker>
                            </v-menu>
                          </v-col>
                        </v-row>
                      </v-container>
                    </v-card-text>
                    <v-card-actions>
                      <v-spacer></v-spacer>
                      <v-btn color="success" @click="updateRegistrationTime"
                        >Xác nhận</v-btn
                      >
                      <v-btn
                        color="error"
                        @click="registrationTimeDialog = false"
                        >Hủy</v-btn
                      >
                    </v-card-actions>
                  </v-card>
                </v-dialog>
                <v-dialog v-model="detailDialog" max-width="800px">
                  <v-card>
                    <v-card-title>
                      <span class="headline"> Danh sách đăng ký học phần </span>
                    </v-card-title>
                    <v-divider></v-divider>
                    <v-card-text>
                      <v-container>
                        <v-row v-if="selectedItem != null">
                          <v-col class="text-dark" cols="12" sm="6" md="4">
                            Mã học phần: {{ selectedItem.id }}
                          </v-col>
                          <v-col class="text-dark" cols="12" sm="6" md="4">
                            Môn học: {{ selectedItem.courseName }}
                          </v-col>
                          <v-col class="text-dark" cols="12" sm="6" md="4">
                            Giáo viên: {{ selectedItem.teacherName }}
                          </v-col>
                          <v-col cols="12">
                            <v-divider></v-divider>
                            <v-data-table
                              :headers="headersS"
                              :items="registrationList"
                              class="elevation"
                              :loading="isLoading"
                              loading-text="Đang tải dữ liệu..."
                              :hide-default-footer="true"
                              :disable-pagination="true"
                            >
                              <template v-slot:[`item.actions`]="{ item }">
                                <v-icon
                                  v-if="
                                    registrationList.length != 0 &&
                                    registrationList[0].hasApproval == false
                                  "
                                  color="red"
                                  title="Xóa"
                                  small
                                  @click="deleteItem(item)"
                                  >mdi-delete</v-icon
                                >
                              </template>
                            </v-data-table>
                          </v-col>
                        </v-row>
                      </v-container>
                    </v-card-text>
                    <v-card-actions>
                      <v-spacer></v-spacer>
                      <v-btn
                        v-if="
                          registrationList.length != 0 &&
                          registrationList[0].hasApproval == false
                        "
                        color="success"
                        @click="approval"
                        >Duyệt</v-btn
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
            <template v-slot:[`item.registerSlot`]="{ item }">
              <span> {{ item.slot }}/{{ item.maxSlot }} </span>
            </template>
            <template v-slot:[`item.actions`]="{ item }">
              <v-icon
                color="blue lighten-1"
                title="Chi tiết"
                small
                class="mr-2"
                @click="regDetail(item)"
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
import MaterialCard from "../../../components/base/MaterialCard";
import datetimeHelper from "../../../helper/datetimeHelper";
import RegistrationTimeService from "../../../services/registration-time.service";
import sectionRegisterService from "../../../services/section-register.service";
import SectionRegisterService from "../../../services/section-register.service";
export default {
  name: "section-register",
  components: {
    MaterialCard,
  },
  data() {
    return {
      selectedItem: null,
      startDayMenu: false,
      endDayMenu: false,
      registrationTimeDialog: false,
      registerItem: [],
      detailDialog: false,
      registrationTime: {},
      registrationList: [],
      isLoading: false,
      headersS: [
        { text: "MSSV", value: "studentUserName", align: "start" },
        { text: "Họ tên", value: "studentName", align: "start" },
        {
          text: "Số điện thoại",
          value: "studentPhoneNumber",
          align: "start",
          sortable: false,
        },
        { text: "", value: "actions", sortable: false, align: "right" },
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
        { text: "Đăng ký", value: "registerSlot", sortable: false },
        { text: "", width: 100, value: "actions", sortable: false },
      ],
    };
  },
  computed: {
    minDate() {
      if (this.registrationTime.startDateTime != null) {
        let dateArray = this.registrationTime.startDateTime.split("-");
        dateArray[2]++;
        let dateStr = dateArray.join("-");
        return dateStr;
      }
      return new Date().toISOString().substr(0, 10);
    },
  },
  methods: {
    getRegistrationTime() {
      RegistrationTimeService.getRegistrationTime().then(
        (response) => {
          this.registrationTime = response.data;
          this.registrationTime.startDateTime = this.registrationTime.startDateTime.substr(
            0,
            10
          );
          this.registrationTime.endDateTime = this.registrationTime.endDateTime.substr(
            0,
            10
          );
        },
        (err) => {
          console.log(err);
        }
      );
    },
    getSectionRegList() {
      SectionRegisterService.getSectionForRegisterByStudentId(0).then(
        (response) => {
          this.registerItem = response.data;
          for (let i = 0; i < this.registerItem.length; i++) {
            let schedule = this.registerItem[i].schedule.split(",");
            this.registerItem[i].schedule = "";
            schedule.forEach((element) => {
              let num = parseInt(element) + 1;
              this.registerItem[i].schedule += "Thứ " + num + ", ";
            });
            this.registerItem[i].startDay = datetimeHelper.getDateFormat(
              this.registerItem[i].startDay
            );
            this.registerItem[i].endDay = datetimeHelper.getDateFormat(
              this.registerItem[i].endDay
            );
          }
        },
        (err) => {
          console.log(err);
        }
      );
    },
    updateRegistrationTime() {
      RegistrationTimeService.setRegistrationTime(this.registrationTime).then(
        () => {
          this.$toast("Thiết lặp thời gian đăng ký học phần thành công!", {
            color: "success",
            x: "right",
            y: "top",
            showClose: true,
            closeIcon: "mdi-close",
          });
          this.registrationTimeDialog = false;
        },
        (err) => {
          console.log(err);
        }
      );
    },
    getDetai() {
      sectionRegisterService
        .getRegisterListBySectionId(this.selectedItem.id)
        .then(
          (response) => {
            console.log(response.data);
            this.registrationList = response.data;
          },
          (err) => {
            console.log(err);
          }
        );
    },
    regDetail(item) {
      console.log(item);
      this.selectedItem = item;
      this.detailDialog = true;
      this.getDetai();
    },
    deleteItem(item) {
      console.log(item);
      SectionRegisterService.deleteStudentRegistration(
        item.studentId,
        item.sectionId
      ).then(
        () => {
          this.$toast("Xóa đăng ký thành công!", {
            color: "success",
            x: "right",
            y: "top",
            showClose: true,
            closeIcon: "mdi-close",
          });
          this.getDetai();
        },
        (err) => {
          console.log(err);
        }
      );
    },
    approval() {
      SectionRegisterService.approvalReg(this.selectedItem.id).then(
        (response) => {
          console.log(response);
          this.$toast("Duyệt đăng ký thành công!", {
            color: "success",
            x: "right",
            y: "top",
            showClose: true,
            closeIcon: "mdi-close",
          });
          this.detailDialog = false;
          this.selectedItem = null;
        },
        (err) => {
          console.log(err);
        }
      );
    },
  },
  mounted() {
    this.getSectionRegList();
    this.getRegistrationTime();
  },
};
</script>