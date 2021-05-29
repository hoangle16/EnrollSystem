class DateTimeHelper {
    getDateFormat(dateString) {
        let newDate = new Date(dateString);
        let year = newDate.getFullYear();
        let month = newDate.getMonth() + 1;
        let day =  newDate.getDate();
        return `${day}-${month}-${year}`;
    }
}

export default new DateTimeHelper();