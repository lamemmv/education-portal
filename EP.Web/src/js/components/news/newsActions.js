import {
    createAction
} from 'redux-actions';

import {
    GET_NEWS_LIST,
    GET_NEWS_LIST_SUCCESS,
    GET_NEWS_LIST_FAILURE,
    GET_NEWS_BYID,
    GET_NEWS_BYID_SUCCESS,
    GET_NEWS_BYID_FAILURE,
    CREATE_NEWS,
    CREATE_NEWS_SUCCESS,
    CREATE_NEWS_FAILURE,
    DELETE_NEWS,
    DELETE_NEWS_SUCCESS,
    DELETE_NEWS_FAILURE,
    UPDATE_NEWS,
    UPDATE_NEWS_SUCCESS,
    UPDATE_NEWS_FAILURE,
    PREPARE_DATA_FOR_NEWS_CREATE
} from './types';

export const getNews = createAction(GET_NEWS_LIST);
export const getNewsSuccess = createAction(GET_NEWS_LIST_SUCCESS);
export const getNewsFailure = createAction(GET_NEWS_LIST_FAILURE);
export const getNewsById = createAction(GET_NEWS_BYID);
export const getNewsByIdSuccess = createAction(GET_NEWS_BYID_SUCCESS);
export const getNewsByIdFailure = createAction(GET_NEWS_BYID_FAILURE);
export const createNews = createAction(CREATE_NEWS);
export const createNewsSuccess = createAction(CREATE_NEWS_SUCCESS);
export const createNewsFailure = createAction(CREATE_NEWS_FAILURE);
export const updateNews = createAction(UPDATE_NEWS);
export const updateNewsSuccess = createAction(UPDATE_NEWS_SUCCESS);
export const updateNewsFailure = createAction(UPDATE_NEWS_FAILURE);
export const deleteNews = createAction(DELETE_NEWS);
export const deleteNewsSuccess = createAction(DELETE_NEWS_SUCCESS);
export const deleteNewsFailure = createAction(DELETE_NEWS_FAILURE);
export const prepareDataForCreatingNews = createAction(PREPARE_DATA_FOR_NEWS_CREATE);