import {
    ADD_NOTIFICATION 
} from './types';

export function addNotification(message, level, title) {
    return {
        type: ADD_NOTIFICATION,
        message,
        level,
        title
    };
}