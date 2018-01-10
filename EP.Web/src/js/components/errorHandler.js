import resources from '../locales/vi.json';

const Service = {
    getErrorMessage(error) {
        let errorCode = error.response.data.statusCode;
        let message = resources["errors"][errorCode];
        return message;
    },
    getMessage(key) {
        return resources["messages"][key];
    }
}

export default Service;