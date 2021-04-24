import axios from 'axios';
import authHeader from './auth-header';
import * as API from '../constant/api';

class TeacherService {
    getTeacherSchedule(teacherId) {
        return axios.get(`${API.TEACHERS}/sections/${teacherId}`, { headers: authHeader() });
    }
}

export default new TeacherService();