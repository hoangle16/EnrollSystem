export default function authHeaderMultiPart() {
    let user = JSON.parse(localStorage.getItem('user'));

    if (user.userInfo && user.token) {
        return {
            Authorization: 'Bearer ' + user.token,
            'Content-Type': 'multipart/form-data'
        };
    } else {
        return {};
    }
}
