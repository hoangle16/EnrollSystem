import axios from 'axios';
import authHeader from './auth-header';
import authHeaderMultiPart from './auth-header-multipart'
import * as API from '../constant/api';

class AttendanceService {
    addTrainingImages(images) {
        return axios.post(`${API.ATTENDANCE}/training`, images, { headers: authHeaderMultiPart() });
    }
    addAttendanceImages(data) {
        return axios.post(`${API.ATTENDANCE}`, data, { headers: authHeaderMultiPart() });
    }
    getAttendanceListBySectionId(sectionId) {
        return axios.get(`${API.ATTENDANCE}/section/${sectionId}`, { headers: authHeader() });
    }
    getAttendanceListBySectionIdAndDate(sectionId, dateString) {
        return axios.get(`${API.ATTENDANCE}/section/${sectionId}/${dateString}`, { headers: authHeader() });
    }
    updateAttendance(sectionId, dateString, data) {
        return axios.put(`${API.ATTENDANCE}/section/${sectionId}/${dateString}`, data, { headers: authHeader() });
    }
    getTrainingImagesByStudentId(studentId) {
        return axios.get(`${API.ATTENDANCE}/training/student/${studentId}`, { headers: authHeader() });
    }
    deleteTrainingImage(trainingImageId) {
        return axios.delete(`${API.ATTENDANCE}/training/${trainingImageId}`, { headers: authHeader() });
    }
    getAttendanceImagesBySectionIdAndDate(sectionId, dateString) {
        return axios.get(`${API.ATTENDANCE}/images/${sectionId}/${dateString}`, { headers: authHeader() });
    }
    getAttendanceImagesBySectionId(sectionId) {
        return axios.get(`${API.ATTENDANCE}/images/${sectionId}`, { headers: authHeader() });
    }
    exportAttendanceReport(sectionId) {
        return axios.get(`${API.ATTENDANCE}/export/${sectionId}`, { headers: authHeader(), responseType: 'blob' });
    }
    changeAttendance(attendanceId) {
        return axios.put(`${API.ATTENDANCE}/${attendanceId}`, {}, { headers: authHeader() });
    }
}

export default new AttendanceService();