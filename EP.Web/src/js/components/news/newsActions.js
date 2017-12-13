import axios from 'axios';
import API from '../api';
import PageSetting from '../../settings/page';

export const CREATE_NEWS = 'CREATE_NEWS';
export const CREATE_NEWS_SUCCESS = 'CREATE_NEWS_SUCCESS';
export const CREATE_NEWS_FAILURE = 'CREATE_NEWS_FAILURE';
export const GET_NEWS_LIST = 'GET_NEWS_LIST';
export const GET_NEWS_LIST_SUCCESS = 'GET_NEWS_LIST_SUCCESS';
export const GET_NEWS_LIST_FAILURE = 'GET_NEWS_LIST_FAILURE';
export const DELETE_NEWS = 'DELETE_NEWS';
export const DELETE_NEWS_SUCCESS = 'DELETE_NEWS_SUCCESS';
export const DELETE_NEWS_FAILURE = 'DELETE_NEWS_FAILURE';

const ROOT_URL = API.getBaseUri();

export function getNews(page) {
    return {
        type: GET_NEWS_LIST
    };
}

export function getNewsSuccess(data) {
    return {
        type: GET_NEWS_LIST_SUCCESS,
        payload: data
    };
}

export function getNewsFailure(error) {
    return {
        type: GET_NEWS_LIST_FAILURE,
        payload: error
    };
}

export function createNews(news) {
    let headers = new Headers({
        'Access-Control-Allow-Origin': '*',
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    });

    const request = axios({
        method: 'post',
        data: news,
        url: `${ROOT_URL}admin/newsManager`,
        headers: headers
    });

    return {
        type: CREATE_NEWS,
        payload: request
    };
}

export function createNewsSuccess(id) {
    return {
        type: CREATE_NEWS_SUCCESS,
        payload: id
    };
}

export function createNewsFailure(error) {
    return {
        type: CREATE_NEWS_FAILURE,
        payload: error
    };
}

export function deleteNews(id) {
    let headers = new Headers({
        'Access-Control-Allow-Origin': '*',
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    });

    const request = axios({
        method: 'delete',
        data: news,
        url: `${ROOT_URL}admin/newsManager`,
        headers: headers
    });

    return {
        type: DELETE_NEWS,
        payload: request
    };
}

export function deleteNewsSuccess() {
    return {
        type: DELETE_NEWS_SUCCESS,
        payload: null
    };
}

export function deleteNewsFailure(error) {
    return {
        type: DELETE_NEWS_FAILURE,
        payload: error
    };
}