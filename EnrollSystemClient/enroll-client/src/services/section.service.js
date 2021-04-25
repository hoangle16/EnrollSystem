import axios from 'axios';
import authHeader from './auth-header';
import * as API from '../constant/api';

class SectionService {
    getSections() {
        return axios.get(`${API.SECTIONS}`, { headers: authHeader() });
    }
    createSection(data) {

        return axios.post(`${API.SECTIONS}`,
            {
                "teacherId": data.teacherId,
                "courseId": data.courseId,
                "startDay": data.startDay,
                "endDay": data.endDay,
                "startTime": data.startTime,
                "endTime": data.endTime,
                "schedule": data.schedule.toString(),
                "maxSlot": data.maxSlot,
                "roomId": data.roomId
            },
            { headers: authHeader() });
    }
}

export default new SectionService();