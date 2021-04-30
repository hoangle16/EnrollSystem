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
    getRegisterListBySectionId(sectionId) {
        return axios.get(`${API.SECTION_REGISTER}/sections/${sectionId}`, { headers: authHeader() });
    }
    deleteStudentRegistration(studentId, sectionId) {
        return axios.delete(`${API.SECTION_REGISTER}/${studentId}/${sectionId}`, { headers: authHeader() });
    }
    approvalReg(sectionId) {
        return axios.put(`${API.SECTION_REGISTER}/approval/${sectionId}`, {}, { headers: authHeader() });
    }
}

export default new SectionRegisterService();