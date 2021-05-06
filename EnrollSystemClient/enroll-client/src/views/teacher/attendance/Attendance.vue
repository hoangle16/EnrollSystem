<template>
  <v-container id="atttendance" fluid tag="section">
    <v-row justify="center">
      <v-col cols="12">
        <material-card>
          <template v-slot:heading>
            <div class="text-h4 font-weight-light">Quản lý điểm danh</div>
          </template>
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
              <v-btn
                style="text-decoration: none"
                id="btn-show"
                small
                icon
                :to="{
                  name: 'admin_attendance_details',
                  params: { sectionId: item.id },
                }"
              >
                <v-icon
                  color="blue lighten-2"
                  title="Chi tiết"
                  small
                  class="mr-2"
                  >mdi-eye</v-icon
                >
              </v-btn>
              <v-icon
                title="Xuất báo cáo điểm danh"
                small
                color="primary"
                class="mr-2"
                @click="exportFile(item)"
                >mdi-calendar-export</v-icon
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
import TeacherService from "../../../services/teacher.service";
import AttendanceService from "../../../services/attendance.service";
export default {
  name: "attendance",
  components: {
    MaterialCard,
  },
  data() {
    return {
      isLoading: false,
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
    };
  },
  computed: {},
  methods: {
    getSections() {
      this.isLoading = true;
      TeacherService.getMySection().then(
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
    exportFile(item) {
      console.log(item);
      AttendanceService.exportAttendanceReport(item.id).then(
        (response) => {
          console.log(response.data);
          const blob = new Blob([response.data], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
          const link = document.createElement("a");
          link.href = URL.createObjectURL(blob);
          link.download = `${item.id}-baocao-diemdanh`;
          link.click();
          URL.revokeObjectURL(link.href);
          this.$toast("Xuất báo cáo điểm danh thành công!", {
            color: "success",
            x: "right",
            y: "top",
            showClose: true,
            closeIcon: "mdi-close",
          });
        },
        (err) => {
          console.log(err);
        }
      );
    },
  },
  mounted() {
    this.getSections();
  },
};
</script>MaterialCard