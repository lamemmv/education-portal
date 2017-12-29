import { handleActions, Action } from 'redux-actions';
import {
    CREATE_NEWS,
    CREATE_NEWS_SUCCESS,
    CREATE_NEWS_FAILURE,
    PREPARE_DATA_FOR_NEWS_CREATE
} from '../types';

const initialState = {
    ingress: '',
    content: '',
    title: '',
    published: false,
    publishedDate: null,
    id: null,
    blobId: null
};

export default handleActions({
    [PREPARE_DATA_FOR_NEWS_CREATE]: (state, action) => {
        return Object.assign({}, state, {
            id: null,
            error: null,
            loading: true,
            blobId: action.payload[0]
        });
    },
    [CREATE_NEWS]: (state, action) => {
        return Object.assign({}, state, {
            id: null,
            error: null,
            loading: true
        });
    },
    [CREATE_NEWS_SUCCESS]: (state, action) => {
        return Object.assign({}, state, {
            id: action.payload,
            error: null,
            loading: false
        });
    },
    [CREATE_NEWS_FAILURE]: (state, action) => {
        let error = action.payload || {
            message: action.payload.message
        }
        return Object.assign({}, state, {
            id: null,
            error: error,
            loading: false
        });
    }

}, initialState);