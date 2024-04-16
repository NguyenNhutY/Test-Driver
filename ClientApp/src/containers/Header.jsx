import React from 'react';
import video from './../assets/pexels-christopher-schultz-5927764 (1080p).mp4';
import '../styles/header.css';

const Header = () => (
  <div className="header section__padding" id="home">
    <div className="header-content">
            <h1 className="gradient__text">Chào mừng bạn đến với trang chủ</h1>
      <br />
      <video autoPlay loop muted>
        <source src={video} type="video/mp4" />
      </video>
    </div>
  </div>
);

export default Header;
