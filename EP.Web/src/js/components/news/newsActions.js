import {
    createAction
} from 'redux-actions';

import {
    GET_NEWS_LIST,
    GET_NEWS_LIST_SUCCESS,
    GET_NEWS_LIST_FAILURE,
    CREATE_NEWS,
    CREATE_NEWS_SUCCESS,
    CREATE_NEWS_FAILURE,
    PREPARE_DATA_FOR_NEWS_CREATE
} from './types';

export const getNews = createAction(GET_NEWS_LIST);
export const getNewsSuccess = createAction(GET_NEWS_LIST_SUCCESS);
export const getNewsFailure = createAction(GET_NEWS_LIST_FAILURE);
export const createNews = createAction(CREATE_NEWS);
export const createNewsSuccess = createAction(CREATE_NEWS_SUCCESS);
export const createNewsFailure = createAction(CREATE_NEWS_FAILURE);
export const prepareDataForCreatingNews = createAction(PREPARE_DATA_FOR_NEWS_CREATE);