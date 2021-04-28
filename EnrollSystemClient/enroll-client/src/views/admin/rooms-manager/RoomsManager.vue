<template>
  <v-container id="rooms-manager" fluid tag="section">
    <v-row justify="center">
      <v-col cols="12">
        <material-card>
          <template v-slot:heading>
            <div class="text-h4 font-weight-light">Quản lý phòng học</div>
          </template>
          <v-data-table
            :headers="headers"
            :items="rooms"
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
                <v-dialog v-model="roomDialog" max-width="500px">
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
                      <span v-if="selectedRoom" class="headline">
                        Cập nhật phòng học
                      </span>
                      <span v-else class="headline"> Phòng học mới </span>
                    </v-card-title>
                    <v-divider></v-divider>
                    <v-card-text>
                      <v-container>
                        <v-row>
                          <v-col cols="12">
                            <v-text-field
                              v-if="!selectedRoom"
                              v-model="roomName"
                              label="Phòng học"
                              hint="Gợi ý: F101 - Tòa nhà F, tầng 1, phòng 01"
                            ></v-text-field>
                            <v-text-field
                              v-else
                              v-model="selectedRoom.name"
                              label="Phòng học"
                              hint="Gợi ý: F101 - Tòa nhà F, tầng 1, phòng 01"
                            ></v-text-field>
                          </v-col>
                        </v-row>
                      </v-container>
                    </v-card-text>
                    <v-card-actions>
                      <v-spacer></v-spacer>
                      <v-btn color="success" @click="createOrUpdateRoom"
                        ><span v-if="!selectedRoom">Thêm</span>
                        <span v-else>Cập nhật</span></v-btn
                      >
                      <v-btn color="error" @click="roomDialogHide">Hủy</v-btn>
                    </v-card-actions>
                    <v-dialog
                      persistent
                      v-model="deleteDialog"
                      max-width="500px"
                    >
                      <v-card>
                        <v-card-title class="headline"
                          >Bạn có chắc muốn xóa phòng học này?</v-card-title
                        >
                        <v-card-actions>
                          <v-spacer></v-spacer>
                          <v-btn color="success" @click="confirmDelete()"
                            >Xóa</v-btn
                          >
                          <v-btn color="error" @click="deleteDialogHide">
                            Hủy</v-btn
                          >
                          <v-spacer></v-spacer>
                        </v-card-actions>
                      </v-card>
                    </v-dialog>
                  </v-card>
                </v-dialog>
                <v-dialog v-model="roomSectionDialog" max-width="900px">
                  <v-card>
                    <v-card-title class="headline"
                      >Tình trạng sử dụng</v-card-title
                    >
                    <v-divider></v-divider>
                    <v-card-text>
                      <v-container>
                        <v-data-table
                          :headers="headerS"
                          :items="currentSections"
                          :items-per-page="15"
                          class="elevation-1"
                          :footer-props="{
                            'items-per-page-text': 'Số hàng mỗi trang',
                          }"
                        >
                          <template v-slot:[`item.genaralSchedule`]="{ item }">
                            <span>
                              {{ item.schedule }} {{ item.startTime }} -
                              {{ item.endTime }}, {{ item.roomName }}
                            </span>
                          </template></v-data-table
                        >
                      </v-container>
                    </v-card-text>
                  </v-card>
                </v-dialog>
              </v-row>
            </template>
            <template v-slot:[`item.actions`]="{ item }">
              <v-btn
                style="text-decoration: none"
                id="btn-show"
                small
                icon
                @click="viewSections(item)"
              >
                <v-icon
                  color="blue lighten-2"
                  title="Tình trạng sử dụng"
                  small
                  class="mr-2"
                  >mdi-calendar</v-icon
                >
              </v-btn>
              <v-icon
                title="Chỉnh sửa"
                small
                color="primary"
                class="mr-2"
                @click="editRoom(item)"
                >mdi-pencil</v-icon
              >
              <v-icon color="red" title="Xóa" small @click="deleteRoom(item)"
                >mdi-delete</v-icon
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
import RoomService from "../../../services/room.service.js";
import datetimeHelper from "../../../helper/datetimeHelper";

export default {
  name: "RoomsManger",
  components: {
    MaterialCard,
  },
  data() {
    return {
      roomSectionDialog: false,
      headers: [
        { text: "STT", value: "serial", sortable: false },
        { text: "Phòng", value: "name" },
        { text: "", value: "actions", sortable: false, align: "right" },
      ],
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
      selectedRoom: null,
      rooms: [],
      isLoading: false,
      roomDialog: false,
      roomName: "",
      deleteDialog: false,
      currentSections: {},
    };
  },
  methods: {
    getRooms() {
      RoomService.getRoom().then(
        (response) => {
          this.rooms = response.data;
          this.rooms.forEach((el, index) => {
            el.serial = index + 1;
          });
        },
        (error) => {
          console.log(error);
        }
      );
    },
    editRoom(item) {
      this.selectedRoom = item;
      console.log(item);
      this.roomDialog = true;
    },
    deleteRoom(item) {
      this.selectedRoom = item;
      this.deleteDialog = true;
    },
    confirmDelete() {
      if (this.selectedRoom != null) {
        RoomService.deleteRoom(this.selectedRoom.id).then(
          (response) => {
            console.log(response);
            this.$toast("Xóa phòng học mới thành công!", {
              color: "success",
              x: "right",
              y: "top",
              showClose: true,
              closeIcon: "mdi-close",
            });
            this.deleteDialog = false;
            this.selectedRoom = null;
            this.getRooms();
          },
          (error) => {
            console.log(error);
          }
        );
      }
    },
    viewSections(item) {
      RoomService.getRoomSections(item.id).then(
        (response) => {
          this.currentSections = response.data.sectionList;
          for (let i = 0; i < this.currentSections.length; i++) {
            //schedule
            let schedule = this.currentSections[i].schedule.split(",");
            this.currentSections[i].schedule = "";
            schedule.forEach((element) => {
              let num = parseInt(element) + 1;
              this.currentSections[i].schedule += "Thứ " + num + ", ";
            });
            // day
            this.currentSections[i].startDay = datetimeHelper.getDateFormat(
              this.currentSections[i].startDay
            );
            this.currentSections[i].endDay = datetimeHelper.getDateFormat(
              this.currentSections[i].endDay
            );
          }
          console.log(this.currentSections);
          this.roomSectionDialog = true;
        },
        (err) => {
          console.log(err);
        }
      );
    },
    createOrUpdateRoom() {
      //update
      if (this.selectedRoom != null) {
        console.log(this.selectedRoom);
        RoomService.updateRoom(
          this.selectedRoom.id,
          this.selectedRoom.name
        ).then(
          (res) => {
            console.log(res);
            this.$toast("Cập nhật phòng học mới thành công!", {
              color: "success",
              x: "right",
              y: "top",
              showClose: true,
              closeIcon: "mdi-close",
            });
            this.selectedRoom = null;
            this.roomDialog = false;
          },
          (error) => {
            console.log(error);
          }
        );
      } else {
        //create
        RoomService.createRoom(this.roomName).then(
          () => {
            this.$toast("Tạo phòng học mới thành công!", {
              color: "success",
              x: "right",
              y: "top",
              showClose: true,
              closeIcon: "mdi-close",
            });
            this.getRooms();
            this.roomDialog = false;
          },
          (error) => {
            console.log(error);
          }
        );
      }
    },
    deleteDialogHide() {
      this.deleteDialog = false;
      this.selectedRoom = null;
    },
    roomDialogHide() {
      this.roomDialog = false;
      this.selectedRoom = null;
    },
  },
  mounted() {
    this.getRooms();
  },
};
</script>