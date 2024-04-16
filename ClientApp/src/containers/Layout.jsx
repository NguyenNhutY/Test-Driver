import React, { Component } from 'react';
import { Container } from 'reactstrap';
import  {NavMenu}  from '../containers/NavMenu';
import  FooterPage  from '../containers/FooterPage';

export class Layout extends Component {
    static displayName = Layout.name;

    render() {
        return (
            <div>
                <NavMenu />
                <Container>
                    {this.props.children}
                </Container>
                <FooterPage />
            </div>
        );
    }
}
