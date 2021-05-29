import axios from 'axios';
import authHeader from './auth-header';
import * as API from '../constant/api';

class CourseService {
    getCourse() {
        return axios.get(`${API.COURSES}`, { headers: authHeader() });
    }
    createCourse(courseName){
        return axios.post(`${API.COURSES}`, { name: courseName }, { headers: authHeader() });
    }
}

export default new CourseService();