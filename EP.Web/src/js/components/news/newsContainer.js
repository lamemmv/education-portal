import {
    connect
} from 'react-redux'
import {
    createNews,
    createNewsSuccess,
    createNewsFailure,
    getNews,
    getNewsSuccess,
    getNewsFailure,
    deleteNews,
    deleteNewsSuccess,
    deleteNewsFailure
} from './newsActions';
import NewsList from './list';

const mapStateToProps = (state) => {
    return {
        createState: state.news.createState,
        listState: state.news.listState
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        getNews: (page) => {
            dispatch(getNews(page));
        },
        createNews: (news) => {
            dispatch(createNews(news));
        },
        deleteNews: (id) => {
            dispatch(deleteNews(id));
        }
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(NewsList);