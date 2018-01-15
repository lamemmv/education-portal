import {
    handleActions,
    Action
} from 'redux-actions';
import {
    OPEN_CONFIRMATION,
    CLOSE_CONFIRMATION
} from './types';

const initialState = {
    showDialog: false,
    request: {

    }
};

export default handleActions({
    [OPEN_CONFIRMATION]: (state, action) => {
        return Object.assign({}, state, {
            request: action.payload,
            showDialog: true
        });
    },
    [CLOSE_CONFIRMATION]: (state, action) => {
        return Object.assign({}, state, {
            showDialog: false
        });
    },
}, initialState);