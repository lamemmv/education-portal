import {
    createAction
} from 'redux-actions';

import {
    GET_NEWS_LIST,
    GET_NEWS_LIST_SUCCESS,
    GET_NEWS_LIST_FAILURE,
    GOTO_CREATE_NEWS,
    INIT_CREATE_NEWS
} from './types';

export const getNews = createAction(GET_NEWS_LIST);
export const getNewsSuccess = createAction(GET_NEWS_LIST_SUCCESS);
export const getNewsFailure = createAction(GET_NEWS_LIST_FAILURE);
export const gotoCreateNews = createAction(GOTO_CREATE_NEWS);
export const initCreateNews = createAction(INIT_CREATE_NEWS);
