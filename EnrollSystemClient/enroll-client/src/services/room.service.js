import axios from 'axios';
import authHeader from './auth-header';
import * as API from '../constant/api';

class RoomService {
    getRoom() {
        return axios.get(`${API.ROOMS}`, { headers: authHeader() });
    }
    getEmptyRoom(model) {
        return axios.get(`${API.ROOMS}/empty-room?StartDay=${model.startDay}&EndDay=${model.endDay}&StartTime=${model.startTime}&EndTime=${model.endTime}&Schedule=${model.schedule}`, { headers: authHeader() });
    }
}

export default new RoomService();