import axios from 'axios';
import authHeader from './auth-header';
import * as API from '../constant/api';

class SectionRegisterService {
    getRegisterListByStudentId(studentId) {
        return axios.get(`${API.SECTION_REGISTER}/students/${studentId}`, { headers: authHeader() });
    }
    getSectionForRegisterByStudentId(studentId) {
        return axios.get(`${API.SECTION_REGISTER}/sectionslist/${studentId}`, { headers: authHeader() });
    }
}

export default new SectionRegisterService();