import axios from 'axios';
import authHeader from './auth-header';
import * as API from '../constant/api';

class UserService {
    getUsers() {
        return axios.get(API.USERS);
    }
    createUser(user) {
        return axios.post(API.USERS, {
            userName: user.userName,
            password: user.password,
            confirmPassword: user.confirmPassword,
            name: user.name,
            gender: user.gender,
            idNumber: user.idNumber,
            phoneNumber: user.phoneNumber,
            address: user.address,
            isActive: user.isActive,
            role: user.role
        },
        { headers: authHeader() });
    }
    getUserById(id) {
        return axios.get(`${API.USERS}/${id}`, { headers: authHeader() });
    }
    
    editUser(id, user) {
        console.log(id, user);
    }
    deleteUser(id) {
        return axios.delete(`${API.USERS}/${id}`, { headers: authHeader() });
    }
    blockUser(id) {
        return axios.put(`${API.BLOCK_USER}/${id}`, "", { headers: authHeader() });
    }
    getProfile() {
        return axios.get(API.PROFILE, { headers: authHeader() });
    }
}

export default new UserService();