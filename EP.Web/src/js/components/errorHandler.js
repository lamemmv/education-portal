import resources from '../locales/vi.json';

const Service = {
    getErrorMesasge(error) {
        let errorCode = error.response.data.statusCode;
        let message = resources["errors"][errorCode];
        return message;
    }
}

export default Service;