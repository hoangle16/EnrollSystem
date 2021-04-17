export default function authHeader() {
    let user = JSON.parse(localStorage.getItem('user'));

    if (user.userInfo && user.token) {
        return { Authorization: 'Bearer' + user.token };
    } else {
        return {};
    }
}