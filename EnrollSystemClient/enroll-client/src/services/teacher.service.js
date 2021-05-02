import axios from 'axios';
import authHeader from './auth-header';
import * as API from '../constant/api';

class TeacherService {
    getTeacherSchedule(teacherId) {
        return axios.get(`${API.TEACHERS}/sections/${teacherId}`, { headers: authHeader() });
    }
    getTeachersAvailable(model) {
        return axios.get(`${API.TEACHERS}/ready?StartDay=${model.startDay}&EndDay=${model.endDay}&StartTime=${model.startTime}&EndTime=${model.endTime}&Schedule=${model.schedule}`, { headers: authHeader() });
    }
    getMySection() {
        return axios.get(`${API.TEACHERS}/sections`, { headers: authHeader() });
    }
}

export default new TeacherService();