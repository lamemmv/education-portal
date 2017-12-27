import { h, Component } from 'preact';
import { Link } from 'react-router-dom';
import { Localizer, Text } from 'preact-i18n';
import * as styles from './list.css';

class NewsList extends Component {

    componentWillMount() {
        this.props.actions.getNews({ page: 1 });
    }

    render() {
        const { items } = this.props.news;
        const { gotoCreateNews } = this.props.actions;
        return (
            <section class='ep-container'>
                <Localizer>
                    <button class="mdc-fab material-icons"
                        aria-label="Favorite"
                        title={<Text id='news.createNews'></Text>}>
                        <Link to='news/create'>
                            <span class="mdc-fab__icon">
                                add
                        </span>
                        </Link>
                    </button>
                </Localizer>
                <div class='shadow-z-1' style={{ marginTop: 10 }}>
                    <table class="table table-hover table-mc-light-blue">
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
                                            <Link to={`/news/${item.id}`}>Link</Link>
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
