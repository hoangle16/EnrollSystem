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
    createRoom(roomName) {
        return axios.post(`${API.ROOMS}`, { name: roomName }, { headers: authHeader() });
    }
    updateRoom(id, roomName){
        return axios.put(`${API.ROOMS}/${id}`, { name: roomName }, { headers: authHeader() });
    }
    deleteRoom(id) {
        return axios.delete(`${API.ROOMS}/${id}`, { headers: authHeader() });
    }
    getRoomSections(roomId) {
        return axios.get(`${API.ROOMS}/${roomId}/sections`, { headers: authHeader() });
    } 
}

export default new RoomService();