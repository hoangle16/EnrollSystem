<template>
  <v-container id="attendance" fluid tag="section">
    <v-row justify="center">
      <v-col cols="12">
        <material-card>
          <template v-slot:heading>
            <div class="text-h4 font-weight-light">Danh sách điểm danh</div>
          </template>
          <v-expansion-panels focusable>
            <v-expansion-panel
              v-for="attendance in attendanceList"
              :key="attendance.date"
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
                <v-data-table
                  :headers="headers"
                  :items="attendance.items"
                  class="elevation-1 mt-1"
                  :loading="isLoading"
                  loading-text="Đang tải dữ liệu..."
                  :hide-default-footer="true"
                  max-height="600px"
                >
                  <template v-slot:[`item.hasAttendance`]="{ item }">
                    <v-chip v-if="item.hasAttendance" color="success">
                      Có
                    </v-chip>
                    <v-chip v-else color="red"> Vắng </v-chip>
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
import MaterialCard from "../../../../components/base/MaterialCard.vue";
import attendanceService from "../../../../services/attendance.service";
export default {
  name: "attendance",
  props: ["sectionId"],
  components: {
    MaterialCard,
  },
  data() {
    return {
      attendanceList: [],
      isLoading: false,
      headers: [
        { text: "MSSV", value: "username", sortable: false },
        { text: "Họ tên", value: "name", sortable: false },
        { text: "SĐT", value: "phoneNumber", sortable: false },
        { text: "Điểm danh", value: "hasAttendance", sortable: false },
        { text: "", value: "actions", sortable: false },
      ],
    };
  },
  methods: {
    getAttendanceList() {
      this.isLoading = true;
      attendanceService.getAttendanceListBySectionId(this.sectionId).then(
        (response) => {
          this.attendanceList = response.data.reverse();
          this.isLoading = false;
        },
        (err) => {
          console.log(err);
        }
      );
    },
  },
  mounted() {
    this.getAttendanceList();
  },
};
</script>