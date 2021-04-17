import axios from 'axios';
import * as API from "../constant/api";

class AuthService {
    login(user) {
        return axios
        .post(API.LOGIN, {
            userName: user.userName,
            password: user.password
        })
        .then(response => {
            if (response.data.token) {
                localStorage.setItem('user', JSON.stringify(response.data));
            }

            return response.data;
        });
    }

    logout() {
        localStorage.removeItem('user');
    }
}

export default new AuthService();