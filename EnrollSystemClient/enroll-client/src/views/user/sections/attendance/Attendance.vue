<template>
  <v-container id="attendance" fluid tag="section">
    <v-row justify="center">
      <v-col cols="12">
        <material-card>
          <template v-slot:heading>
            <div class="text-h4 font-weight-light">Danh sách điểm danh</div>
          </template>
          <v-data-table
            :headers="headers"
            :items="attendanceList"
            class="elevation-1 mt-1"
            :loading="isLoading"
            loading-text="Đang tải dữ liệu..."
            :footer-props="{
              'items-per-page-text': 'Số hàng mỗi trang',
            }"
          >
          <template v-slot:top>
            <v-row class="px-3">
              <v-col cols="12" class="pb-0">
                <span class="font-weight-bold"> Học phần: {{ section.courseName }} </span>
              </v-col>
              <v-col cols="12">
                <span class="font-weight-bold"> Giáo viên: {{ section.teacherName }} </span>
              </v-col>
            </v-row>
          </template>
          <template v-slot:[`item.date`]="{ item }">
            <span> {{ item.date.substr(0,10).split("-").reverse().join("-") }} </span>
          </template>
            <template v-slot:[`item.hasAttendance`]="{ item }">
              <v-chip dark v-if="item.hasAttendance" color="success"> Có </v-chip>
              <v-chip dark v-else color="red"> Vắng </v-chip>
            </template>
          </v-data-table>
        </material-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import MaterialCard from "../../../../components/base/MaterialCard.vue";
import attendanceService from "../../../../services/attendance.service";
import sectionService from '../../../../services/section.service';
export default {
  name: "attendance",
  props: ["sectionId"],
  components: {
    MaterialCard,
  },
  data() {
    return {
      section: {},
      attendanceList: [],
      isLoading: false,
      headers: [
        { text: "Ngày", value: "date", sortable: true },
        { text: "Điểm danh", value: "hasAttendance", sortable: false },
      ],
    };
  },
  methods: {
    getSectionInfo() {
      sectionService.getSectionById(this.sectionId).then(
        (response) => {
          this.section = response.data;
        },
        (err) => {
          console.log(err);
        }
      )
    },
    getAttendanceList() {
      this.isLoading = true;
      attendanceService.getMyAttendanceList(this.sectionId).then(
        (response) => {
          this.attendanceList = response.data;
          this.isLoading = false;
        },
        (err) => {
          console.log(err);
        }
      );
    },
  },
  mounted() {
    this.getSectionInfo();
    this.getAttendanceList();
  },
};
</script>