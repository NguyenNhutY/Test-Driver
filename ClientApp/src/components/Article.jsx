import React from 'react';
import './../styles/article.css';

const Article = ({ imgUrl, title, text }) => (
  <div className="blog-container_article">
    <div className="blog-container_article-image">
      <img src={imgUrl} alt="blog_image" />
    </div>
    <div className="blog-container_article-content">
      <div>
        <p>{title}</p>
        <h3>{text}</h3>
      </div>
      <p>Đọc Thêm</p>
    </div>
  </div>
);

export default Article;
