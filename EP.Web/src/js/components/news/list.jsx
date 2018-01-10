import { h, Component } from 'preact';
import { Link } from 'react-router-dom';
import { Localizer, Text } from 'preact-i18n';
import * as styles from './list.css';

class NewsList extends Component {

    constructor(props, context) {
        super(props, context);
        this.createNews = this.createNews.bind(this);
    }

    createNews() {
        this.context.router.history.push('/news/create');
    }

    componentWillMount() {
        this.props.actions.getNews({ page: 1 });
    }

    render() {
        const { items } = this.props.news;
        return (
            <section class='container'>
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
                <div style={{ margin: 10 }}>
                    <table class="table">
                        <thead>
                            <tr>
                                <th><Text id='news.title'></Text></th>
                                <th><Text id='news.ingress'></Text></th>
                                <th><Text id='news.content'></Text></th>
                                <th><Text id='news.publishedDate'></Text></th>
                                <th><Text id='news.link'></Text></th>
                            </tr>
                        </thead>
                        <tbody>
                            {items.map((item) => {
                                return (
                                    <tr>
                                        <td data-title="Title">{item.title}</td>
                                        <td data-title="Ingress">{item.ingress}</td>
                                        <td data-title="Content">{item.content}</td>
                                        <td data-title="PublishedDate">{item.createdOn}</td>
                                        <td data-title="Link">
                                            <Link to={`news/${item.id}`}>Link</Link>
                                        </td>
                                    </tr>
                                );
                            })}
                        </tbody>
                    </table>
                </div>
            </section>
        );
    }
}

export default NewsList;
