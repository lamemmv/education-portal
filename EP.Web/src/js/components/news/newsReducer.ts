import { handleActions, Action } from 'redux-actions';
import {
    GET_NEWS_LIST,
    GET_NEWS_LIST_FAILURE,
    GET_NEWS_LIST_SUCCESS
} from './newsActions';

import { NewsModel, NewsFetchSuccess, NewsFetchFail, NewsFilter } from './models';


export default handleActions({
    [GET_NEWS_LIST]: (state: NewsModel[], action: Action<NewsFilter>) => {
        return [{
            items: [],
            page: action.payload.page,
            error: null,
            loading: true
        }, ...state];
    },
    [GET_NEWS_LIST_SUCCESS]: (state: NewsModel[], action: Action<NewsFetchSuccess>) => {
        return [{
            items: action.payload.items,
            page: action.payload.page,
            error: null,
            loading: true
        }, ...state];
    },
    [GET_NEWS_LIST_FAILURE]: (state: NewsModel[], action: Action<NewsFetchFail>) => {
        let error = action.payload || {
            message: action.payload.message
        }
        return [{
            items: [],
            page: action.payload.page,
            error: error,
            loading: true
        }, ...state];
    },

}, {});