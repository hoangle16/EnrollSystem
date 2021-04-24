import axios from 'axios';
import authHeader from './auth-header';
import * as API from '../constant/api';

class StudentService {
    getStudentSchedule(studentId) {
        return axios.get(`${API.STUDENTS}/${studentId}/section`, { headers: authHeader() });
    }
}

export default new StudentService();