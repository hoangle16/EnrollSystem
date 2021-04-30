import axios from 'axios';
import authHeader from './auth-header';
import * as API from '../constant/api';

class RegistrationTimeService {
    getRegistrationTime() {
        return axios.get(`${API.REGISTRATION_TIME}`, { headers: authHeader() });
    }
    setRegistrationTime(data) {
        return axios.put(`${API.REGISTRATION_TIME}`, {
            startDateTime: data.startDateTime,
            endDateTime: data.endDateTime,
        },
            { headers: authHeader() });
    }
    closeRegistrationTime() {
        return axios.put(`${API.REGISTRATION_TIME}/close`, {}, { headers: authHeader() });
    }
}

export default new RegistrationTimeService();