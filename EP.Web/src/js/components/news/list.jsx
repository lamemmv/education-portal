import { h, Component } from 'preact';
import { Link } from 'react-router-dom';
import { Localizer, Text } from 'preact-i18n';
import * as styles from './list.css';
import EPPAgination from '../pagination/container';
import Spinner from '../spinner/container';

class NewsList extends Component {

    constructor(props, context) {
        super(props, context);
        this.createNews = this.createNews.bind(this);
    }

    createNews() {
        this.context.router.history.push('news/create');
    }

    componentWillMount() {
        this.props.actions.getNews({ page: 1 });
    }

    render() {
        const { items, pages, page, size, loading } = this.props.news;
        const { getNews, askToOpenConfirmation } = this.props.actions;
        return (
            <section class='container'>
                <Spinner loading={loading} />
                <form>
                    <Localizer>
                        <button type="button"
                            class="btn btn-raised btn-info"
                            title={<Text id='news.createNews'></Text>}
                            onClick={this.createNews}>
                            <i class="material-icons ep-icon">add</i>
                            <span><Text id='news.createNews'></Text></span>
                        </button>
                    </Localizer>
                </form>
                <div style={{ marginTop: 10 }}>
                    <table class="table table-hover table-striped ep-table">
                        <thead>
                            <tr>
                                <th><Text id='news.title'></Text></th>
                                <th><Text id='news.ingress'></Text></th>
                                <th><Text id='news.published'></Text></th>
                                <th><Text id='news.publishedDate'></Text></th>
                                <th></th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            {items.map((item) => {
                                return (
                                    <tr>
                                        <td data-title="Title" title={item.title}>{item.title}</td>
                                        <td data-title="Ingress" title={item.ingress}>{item.ingress}</td>
                                        <td data-title="Published" title={item.published}>
                                            {
                                                item.published ?
                                                    <i class='material-icons'>check</i> :
                                                    null
                                            }</td>
                                        <td data-title="PublishedDate" title={item.createdOn}>{item.createdOn}</td>
                                        <td data-title="Link">
                                            <Link to={`news/${item.id}`}><Text id='news.detail'></Text></Link>
                                        </td>
                                        <td data-title="Delete">
                                            <button type="button"
                                                class="btn btn-raised btn-danger"
                                                onClick={() => askToOpenConfirmation()}>
                                                <Text id='files.delete'></Text>
                                            </button>
                                        </td>
                                    </tr>
                                );
                            })}
                        </tbody>
                    </table>
                </div>
                <EPPAgination action={getNews} pages={pages} currentPage={page} size={size} />
            </section>
        );
    }
}

export default NewsList;
