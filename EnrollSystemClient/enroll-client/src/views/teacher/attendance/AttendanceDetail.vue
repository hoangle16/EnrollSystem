<template>
  <v-container id="attendance-detail" fluid tag="section">
    <v-row justify="center">
      <v-col cols="12" md="12">
        <material-card>
          <template v-slot:heading>
            <div class="text-h4 font-weight-light">Danh sách điểm danh</div>
          </template>
          <v-row>
            <v-col cols="12" class="d-flex mb-4">
              <v-spacer></v-spacer>
              <!-- <v-btn color="success">Điểm danh</v-btn> -->
              <v-dialog v-model="attendanceDialog" max-width="600px">
                <template v-slot:activator="{ on, attrs }">
                  <v-btn
                    small
                    color="success"
                    title="Điểm danh"
                    v-bind="attrs"
                    v-on="on"
                  >
                    <v-icon class="mr-1">mdi-account-check</v-icon>
                    Điểm danh
                  </v-btn>
                </template>
                <v-card>
                  <v-card-title>
                    <span class="headline"> Điểm danh </span>
                  </v-card-title>
                  <v-divider></v-divider>
                  <v-card-text>
                    <v-container>
                      <v-row>
                        <v-col cols="12">
                          <v-menu
                            v-model="attendanceDateMenu"
                            :close-on-content-click="false"
                            :nudge-right="40"
                            transition="scale-transition"
                            offset-y
                            min-width="auto"
                          >
                            <template v-slot:activator="{ on, attrs }">
                              <v-text-field
                                v-model="newAttendance.dateTime"
                                label="Ngày điểm danh"
                                readonly
                                v-bind="attrs"
                                v-on="on"
                              ></v-text-field>
                            </template>
                            <v-date-picker
                              v-model="newAttendance.dateTime"
                              @input="attendanceDateMenu = false"
                              locale="vi-vn"
                            ></v-date-picker>
                          </v-menu>
                        </v-col>
                        <v-col cols="12">
                          <v-file-input
                            v-model="newAttendance.images"
                            small-chips
                            accept="image/png, image/jpeg, image/bmp"
                            placeholder="Chọn ảnh để điểm danh"
                            prepend-icon="mdi-camera"
                            label="Chọn ảnh điểm danh"
                            clearable
                            multiple
                            @change="inputChanged"
                            :rules="[rules.required]"
                          >
                            <template v-slot:selection="{ text, index }">
                              <v-chip
                                small
                                text-color="white"
                                color="#295671"
                                close
                                @click:close="remove(index)"
                              >
                                {{ text }}
                              </v-chip>
                            </template></v-file-input
                          >
                        </v-col>
                      </v-row>
                    </v-container>
                  </v-card-text>
                  <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn
                      color="success"
                      @click="attendance"
                      :loading="loading"
                    >
                      <!-- <v-progress-circular v-if="loading"
                        indeterminate
                      ></v-progress-circular> -->
                      <span>Xác nhận</span></v-btn
                    >
                    <v-btn color="error" @click="attendanceDialogHide"
                      >Hủy</v-btn
                    >
                  </v-card-actions>
                </v-card>
              </v-dialog>
            </v-col>
          </v-row>
          <v-expansion-panels focusable>
            <v-expansion-panel
              v-for="attendance in attendanceList"
              :key="attendance.date"
              @click="loadAttendanceImage(attendance)"
            >
              <v-expansion-panel-header>
                <span class="font-weight-bold">
                  Ngày
                  {{
                    attendance.date
                      .substring(0, 10)
                      .split("-")
                      .reverse()
                      .join("-")
                  }}
                </span>
              </v-expansion-panel-header>
              <v-expansion-panel-content>
                <v-expansion-panels class="my-2">
                  <v-expansion-panel>
                    <v-expansion-panel-header>
                      <span class="font-weight-bold">Ảnh điểm danh</span>
                    </v-expansion-panel-header>
                    <v-expansion-panel-content>
                      <v-row class="mt-2">
                        <v-col
                          v-for="img in images"
                          :key="img.id"
                          class="d-flex child-flex"
                          cols="12"
                          sm="6"
                          md="4"
                        >
                          <v-img
                            @click.stop="chooseImage(img)"
                            :src="img.path"
                            :lazy-src="img.path"
                            aspect-ratio="1"
                            class="grey lighten-2"
                          >
                            <template v-slot:placeholder>
                              <v-row
                                class="fill-height ma-0"
                                align="center"
                                justify="center"
                              >
                                <v-progress-circular
                                  indeterminate
                                  color="grey lighten-5"
                                ></v-progress-circular>
                              </v-row>
                            </template>
                          </v-img>
                          <v-dialog v-model="imageDialog" max-width="720">
                            <v-card>
                              <v-card-text class="p-0">
                                <v-img
                                  :src="currentImg.path"
                                  :aspect-ratio="1"
                                  max-width="720"
                                  class="grey lighten-2"
                                ></v-img>
                              </v-card-text>
                            </v-card>
                          </v-dialog>
                        </v-col>
                      </v-row>
                    </v-expansion-panel-content>
                  </v-expansion-panel>
                </v-expansion-panels>
                <v-data-table
                  :headers="headers"
                  :items="attendance.items"
                  class="elevation-1 mt-1"
                  :loading="isLoading"
                  loading-text="Đang tải dữ liệu..."
                  :hide-default-footer="true"
                  :disable-pagination="true"
                  max-height="600px"
                >
                  <template v-slot:[`item.hasAttendance`]="{ item }">
                    <v-chip
                      @click="changeAttendance(item)"
                      v-if="item.hasAttendance"
                      color="success"
                    >
                      Có
                    </v-chip>
                    <v-chip @click="changeAttendance(item)" v-else color="red">
                      Vắng
                    </v-chip>
                  </template>
                </v-data-table>
              </v-expansion-panel-content>
            </v-expansion-panel>
          </v-expansion-panels>
        </material-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import MaterialCard from "../../../components/base/MaterialCard.vue";
import AttendanceService from "../../../services/attendance.service";
import * as API from "../../../constant/api.js";
export default {
  name: "attendanceDetail",
  props: ["sectionId"],
  components: {
    MaterialCard,
  },
  data() {
    return {
      // currImages: [],
      loading: false,
      currentImg: {},
      newAttendance: { dateTime: new Date().toISOString().substring(0, 10) },
      attendanceDateMenu: false,
      attendanceDialog: false,
      selectedAttendance: null,
      imageDialog: false,
      attendanceList: [],
      isLoading: false,
      images: [],
      headers: [
        { text: "MSSV", value: "username", sortable: false },
        { text: "Họ tên", value: "name", sortable: false },
        { text: "SĐT", value: "phoneNumber", sortable: false },
        { text: "Điểm danh", value: "hasAttendance", sortable: false },
        { text: "", value: "actions", sortable: false },
      ],
      rules: { required: (v) => !!v || "Vui lòng chọn ít nhất 1 ảnh" },
    };
  },
  computed: {},
  methods: {
    getAttendanceList() {
      this.isLoading = true;
      AttendanceService.getAttendanceListBySectionId(this.sectionId).then(
        (response) => {
          this.attendanceList = response.data.reverse();
          this.isLoading = false;
        },
        (err) => {
          console.log(err);
        }
      );
    },
    changeAttendance(item) {
      AttendanceService.changeAttendance(item.id).then(
        (response) => {
          //console.log(response.data);
          item.hasAttendance = response.data.hasAttendance;
        },
        (err) => {
          console.log(err);
        }
      );
    },
    loadAttendanceImage(attendance) {
      this.selectedAttendance = attendance;
      AttendanceService.getAttendanceImagesBySectionIdAndDate(
        attendance.sectionId,
        attendance.date.substring(0, 10)
      ).then(
        (response) => {
          this.images = [];
          response.data.forEach((el) => {
            let src = `${API.SERVER}/${el.path}`;
            this.images.push({ id: el.id, imageId: el.imageId, path: src });
          });
          //console.log(this.images);
        },
        (err) => {
          console.log(err);
        }
      );
    },
    remove(index) {
      this.newAttendance.images.splice(index, 1);
    },
    inputChanged() {
      // this.newAttendance.images = [
      //   ...this.currImages,
      //   ...this.newAttendance.images,
      // ];
      console.log(this.newAttendance.images);
    },
    attendanceDialogHide() {
      this.attendanceDialog = false;
      this.newAttendance = {};
    },
    attendance() {
      this.loading = true;
      //console.log(this.newAttendance);
      let formData = new FormData();
      formData.append("sectionId", this.sectionId);
      //formData.append("images[]", this.newAttendance.images);
      this.newAttendance.images.forEach((el) => {
        formData.append("images", el);
      });
      formData.append("dateTime", this.newAttendance.dateTime);
      AttendanceService.addAttendanceImages(formData).then(
        () => {
          this.$toast("Điểm danh hoàn thành!", {
            color: "success",
            x: "right",
            y: "top",
            showClose: true,
            closeIcon: "mdi-close",
          });
          this.getAttendanceList();
          if (this.selectedAttendance != null) {
            this.loadAttendanceImage(this.selectedAttendance);
          }
          this.attendanceDialogHide();
          this.loading = false;
        },
        (err) => {
          console.log(err);
          this.loading = false;
        }
      );
    },
    chooseImage(img) {
      this.currentImg = img;
      this.imageDialog = true;
    },
  },
  mounted() {
    this.getAttendanceList();
    this.newAttendance.images;
  },
};
</script>