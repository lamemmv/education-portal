import {
    ADD_NOTIFICATION 
} from './types';

export default function notification(state = {}, action) {
    switch (action.type) {
        case ADD_NOTIFICATION:
            return Object.assign({}, state, {
                message: action.message,
                level: action.level,
                title: action.title
            });

        default:
            console.debug('notification reducer :: hit default', action.type);
            return state;
    }
}