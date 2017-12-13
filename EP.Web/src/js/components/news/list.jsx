import React, { Component } from 'react';
import { render } from 'react-dom';
import { Link } from 'react-router-dom';
import { Container, Image, Table, Button } from 'semantic-ui-react';

class NewsList extends Component {
    
    componentWillMount() {
        this.props.getNews(this.props.listState.page);
    }

    renderTableBody(items) {
        const { askForDeleting } = this.props;
        return (
            items.map((news) => {
                return (<Table.Row key={news.id}>
                    <Table.Cell>
                        {news.title}
                    </Table.Cell>
                    <Table.Cell>
                        <Link to={`/news/${news.id}`}>
                            <Image avatar src={require('../../../assets/images/image.png')} />
                        </Link>
                    </Table.Cell>
                    <Table.Cell>
                        <Button onClick={() => askForDeleting(news.id)}>Delete</Button>
                    </Table.Cell>
                </Table.Row>)
            })
        );
    }

    render() {
        const { items } = this.props.listState;
        return (
            <Container >
                <Table color={'red'} key={'red'}>
                    <Table.Header>
                        <Table.Row>
                            <Table.HeaderCell>File name</Table.HeaderCell>
                            <Table.HeaderCell>Preview</Table.HeaderCell>
                            <Table.HeaderCell></Table.HeaderCell>
                        </Table.Row>
                    </Table.Header>
                    <Table.Body>
                        {this.renderTableBody(items)}
                    </Table.Body>
                </Table>
            </Container>
        );
    }
}

export default NewsList;
