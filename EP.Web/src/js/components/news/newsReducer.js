import { handleActions, Action } from 'redux-actions';
import {
    GET_NEWS_LIST,
    GET_NEWS_LIST_FAILURE,
    GET_NEWS_LIST_SUCCESS,
    GOTO_CREATE_NEWS
} from './types';

const initialState = {
    items: [],
    page: 1,
    loading: false,
    error: null
};

export default handleActions({
    [GET_NEWS_LIST]: (state, action) => {
        return Object.assign({}, state, {
            items: [],
            page: action.payload.page,
            error: null,
            loading: true
        });
    },
    [GET_NEWS_LIST_SUCCESS]: (state, action) => {
        return Object.assign({}, state, {
            items: action.payload.items ? action.payload.items : [],
            page: action.payload.page,
            error: null,
            loading: false
        });
    },
    [GET_NEWS_LIST_FAILURE]: (state, action) => {
        let error = action.payload || {
            message: action.payload.message
        }
        return Object.assign({}, state, {
            items: [],
            page: action.payload.page,
            error: error,
            loading: false
        });
    },
    [GOTO_CREATE_NEWS]: (state, action) => {
        return Object.assign({}, state, {
            items: [],
            error: null,
            loading: false
        });
    },

}, initialState);