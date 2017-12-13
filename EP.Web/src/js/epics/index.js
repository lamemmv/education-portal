import NewsEpic from '../components/news/newsEpic';
import FileEpic from '../components/posts/fileEpic';
import { combineEpics } from 'redux-observable';

const epics = [
    NewsEpic,
    FileEpic
];

const rootEpics = combineEpics(...Object.values(epics));;

export default rootEpics;