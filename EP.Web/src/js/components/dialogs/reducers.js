import {
    handleActions,
    Action
} from 'redux-actions';
import {
    ASK_TO_OPEN_CONFIRMATION,
    OPEN_CONFIRMATION,
    CLOSE_CONFIRMATION
} from './types';

const initialState = {
    showDialog: false
};

export default handleActions({
    [ASK_TO_OPEN_CONFIRMATION]: (state, action) => {
        return Object.assign({}, state, {
            showDialog: true
        });
    },
    [OPEN_CONFIRMATION]: (state, action) => {
        return Object.assign({}, state, {
            showDialog: true
        });
    },
    [CLOSE_CONFIRMATION]: (state, action) => {
        return Object.assign({}, state, {
            showDialog: false
        });
    },
}, initialState);