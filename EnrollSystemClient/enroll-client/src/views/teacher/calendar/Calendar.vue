<template>
  <v-container id="calendar" fluid tag="section">
    <v-row justify="center">
      <v-col cols="12">
        <material-card>
          <template v-slot:heading>
            <div class="text-h4 font-weight-light">Lịch giảng dạy</div>
          </template>
          <v-sheet tile height="54" class="d-flex">
            <v-btn icon class="ma-2" @click="$refs.calendar.prev()">
              <v-icon>mdi-chevron-left</v-icon>
            </v-btn>
            <v-btn icon class="ma-2" @click="$refs.calendar.next()">
              <v-icon>mdi-chevron-right</v-icon>
            </v-btn>
            <v-flex shrink>
              <v-select
                v-model="type"
                :items="types"
                hide-details
                class="ma-2"
                label="Lịch theo"
              ></v-select>
            </v-flex>
            <v-spacer></v-spacer>
          </v-sheet>
          <v-sheet class="mt-2" height="600px">
            <v-calendar
              ref="calendar"
              locale="vi-vn"
              v-model="value"
              :weekdays="weekday"
              :type="type"
              :events="events"
              event-overlap-mode="stack"
              first-time="06:00"
              :event-overlap-threshold="30"
              :event-color="getEventColor"
            >
              <template v-slot:event="{ eventSummary, eventParsed }">
                <v-tooltip open-delay="50" bottom>
                  <template v-slot:activator="{ on, attrs }">
                    <span v-bind="attrs" v-on="on" v-html="eventSummary()">
                    </span>
                  </template>
                  <span>{{ eventParsed.input.name }} </span>
                  <br/>
                  <span> {{ eventParsed.start.time }}  - {{ eventParsed.end.time }} </span>
                </v-tooltip>
              </template>
            </v-calendar>
          </v-sheet>
        </material-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import MaterialCard from "../../../components/base/MaterialCard.vue";
import TeacherService from "../../../services/teacher.service";
export default {
  name: "calendar",
  components: {
    MaterialCard,
  },
  data() {
    return {
      show: false,
      sections: [],
      type: "month",
      types: [
        { text: "Ngày", value: "day" },
        { text: "Tuần", value: "week" },
        { text: "Tháng", value: "month" },
      ],
      value: "",
      weekday: [1, 2, 3, 4, 5, 6, 0],
      events: [],
      colors: ["blue", "indigo", "deep-purple", "cyan", "green", "orange"],
    };
  },
  computed: {},
  methods: {
    getEventColor(event) {
      return event.color;
    },
    getEvents() {
      this.events = [];
      this.sections.forEach((section) => {
        let colorIndex = this.rnd(0, this.colors.length - 1);
        section.events.forEach((ev) => {
          let newEvent = {
            name: ev.name,
            start: ev.start,
            end: ev.end,
            color: this.colors[colorIndex],
            timed: true
          };
          this.events.push(newEvent);
        });
      });
    },
    rnd(a, b) {
      return Math.floor((b - a + 1) * Math.random()) + a;
    },
    getCalendarItem() {
      TeacherService.getCalendar().then(
        (response) => {
          this.sections = response.data;
          //console.log("calendar", this.sections);
          this.getEvents();
        },
        (err) => {
          console.log(err);
        }
      );
    },
  },
  mounted() {
    this.getCalendarItem();
  },
};
</script>