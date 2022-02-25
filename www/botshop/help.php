<!DOCTYPE html>
<html lang="ru">
<head>
  <script type="text/javascript" src="scripts/jquery.js"></script>
  <?php
    include_once "head.php" ;
  ?>
  <link rel="stylesheet" href="/style/help.css">
</head>
<body>
  <section class="section__helpbanner">
    <div class="helpbanner">
      <div class="helpbanner__titlesite">
              <a href="/">
                <svg width="50" height="38" viewBox="0 0 50 38" fill="none" xmlns="http://www.w3.org/2000/svg">
                  <path fill-rule="evenodd" clip-rule="evenodd" d="M22.3643 0.0768072C17.7162 0.552829 13.2835 2.34851 9.47123 5.29977C8.31046 6.19843 6.16958 8.33931 5.27091 9.50008C2.49575 13.0849 0.817846 17.0318 0.140802 21.5677C0.0284591 22.3207 0 23.0109 0 24.9875C0 27.6954 0.0605346 28.3307 0.520204 30.4513C0.962815 32.4929 1.90786 35.0386 2.89206 36.8403L3.29002 37.5689L3.44489 37.312C4.99292 34.744 7.1456 32.2605 9.30142 30.5553C9.63263 30.2933 9.91164 30.0722 9.92154 30.0638C9.93137 30.0555 9.82028 29.7148 9.67469 29.3066C9.04599 27.5445 8.75542 25.819 8.75542 23.8476C8.75542 21.914 9.04065 20.1872 9.64489 18.463C9.88899 17.7663 10.7641 15.868 10.8411 15.868C10.8655 15.868 11.0785 16.1598 11.3144 16.5166C13.7145 20.1447 17.497 22.7096 21.7354 23.5827C22.5668 23.754 22.8259 23.769 24.9587 23.769C27.0914 23.769 27.3505 23.754 28.1819 23.5827C32.4203 22.7096 36.2028 20.1447 38.6029 16.5166C38.8388 16.1599 39.0518 15.868 39.0762 15.868C39.1532 15.868 40.0283 17.7663 40.2724 18.463C40.8767 20.1872 41.1619 21.914 41.1619 23.8476C41.1619 25.819 40.8713 27.5445 40.2426 29.3066C40.097 29.7148 39.9859 30.0555 39.9958 30.0638C40.0057 30.0722 40.2847 30.2933 40.6159 30.5553C42.7719 32.2606 44.9238 34.7434 46.4727 37.3125L46.6279 37.5699L47.0675 36.7622C48.3612 34.3849 49.3304 31.3955 49.7765 28.4073C49.979 27.05 49.979 22.925 49.7765 21.5677C49.0995 17.0318 47.4215 13.0849 44.6464 9.50008C43.7477 8.33931 41.6068 6.19843 40.4461 5.29977C36.5789 2.30605 32.1783 0.540329 27.4071 0.0680808C26.4494 -0.0267306 23.3224 -0.021306 22.3643 0.0768072ZM10.8215 18.466C10.2575 19.6333 9.70708 21.299 9.70708 21.838C9.70708 22.0763 11.5033 22.8825 12.8517 23.2495C14.037 23.572 14.7663 23.651 16.557 23.651C17.6508 23.651 18.2763 23.6224 18.2763 23.5724C18.2763 23.5292 18.2226 23.4938 18.1572 23.4938C17.9354 23.4938 16.0923 22.4955 15.3149 21.9544C13.9872 21.0302 12.5649 19.546 11.7029 18.1853C11.4981 17.862 11.3102 17.598 11.2853 17.5984C11.2605 17.5989 11.0517 17.9893 10.8215 18.466ZM38.2144 18.1853C37.3524 19.546 35.9301 21.0302 34.6024 21.9544C33.825 22.4955 31.9819 23.4938 31.7601 23.4938C31.6947 23.4938 31.641 23.5292 31.641 23.5724C31.641 23.6224 32.2665 23.651 33.3603 23.651C35.151 23.651 35.8803 23.572 37.0656 23.2495C38.414 22.8825 40.2102 22.0763 40.2102 21.838C40.2102 21.5988 39.9917 20.7407 39.7467 20.0178C39.5135 19.3297 38.7166 17.6001 38.632 17.5984C38.6071 17.598 38.4192 17.862 38.2144 18.1853ZM20.6662 25.8138C20.3611 26.1188 20.2417 26.4945 20.2417 27.1495C20.2417 28.1255 20.5844 28.6825 21.1851 28.6825C21.7857 28.6825 22.1285 28.1255 22.1285 27.1495C22.1285 26.1734 21.7857 25.6164 21.1851 25.6164C20.9465 25.6164 20.8125 25.6674 20.6662 25.8138ZM28.2134 25.8138C27.9083 26.1188 27.7888 26.4945 27.7888 27.1495C27.7888 28.1255 28.1316 28.6825 28.7322 28.6825C29.3329 28.6825 29.6756 28.1255 29.6756 27.1495C29.6756 26.1734 29.3329 25.6164 28.7322 25.6164C28.4937 25.6164 28.3597 25.6674 28.2134 25.8138ZM24.8562 29.5276C24.7833 29.891 24.4634 30.284 24.0728 30.4907C23.8685 30.5988 23.7011 30.7145 23.701 30.7479C23.7009 30.7813 23.8669 30.9139 24.07 31.0427C24.3836 31.2416 24.5173 31.2768 24.9587 31.2768C25.4 31.2768 25.5337 31.2416 25.8473 31.0427C26.0504 30.9139 26.2164 30.7813 26.2163 30.7479C26.2162 30.7145 26.0488 30.5988 25.8445 30.4907C25.4539 30.284 25.134 29.891 25.0611 29.5276C25.0372 29.4087 24.9911 29.3114 24.9587 29.3114C24.9262 29.3114 24.8801 29.4087 24.8562 29.5276Z" fill="#353A40"/>
                </svg>
                <span class="helpbanner__title">BOTSHOP</span>
              </a>
        </div>
        <div class="helpbanner-welcome">
          Добро пожаловать в поддержку!
        </div>
    </div>
  </section>
  <section class="section__cards">
    <div class="cards">
      <div class="card">
        <div class="card-header">
          <div class="card-title">
            Название пункта помощи
          </div>
          <div class="card-ico">
            <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
            <path d="M7.41 8.58997L12 13.17L16.59 8.58997L18 9.99997L12 16L6 9.99997L7.41 8.58997Z" fill="#323232"/>
            </svg>
          </div>
        </div>
        <div class="card-text">
          Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque mattis ultricies ipsum. Donec vel consequat ex, id iaculis quam. Proin at massa nunc. Aliquam lacus orci, imperdiet vel sem quis, gravida facilisis mauris. Pellentesque nulla nibh, vehicula vitae sagittis et, lobortis a lorem. Fusce non finibus nisi, malesuada faucibus libero. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris eget risus ante. In ut enim et ex semper rutrum quis ac mi. Suspen
        </div>
      </div>
      <div class="card">
        <div class="card-header">
          <div class="card-title">
            Название пункта помощи
          </div>
          <div class="card-ico">
            <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
            <path d="M7.41 8.58997L12 13.17L16.59 8.58997L18 9.99997L12 16L6 9.99997L7.41 8.58997Z" fill="#323232"/>
            </svg>
          </div>
        </div>
        <div class="card-text">
          Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque mattis ultricies ipsum. Donec vel consequat ex, id iaculis quam. Proin at massa nunc. Aliquam lacus orci, imperdiet vel sem quis, gravida facilisis mauris. Pellentesque nulla nibh, vehicula vitae sagittis et, lobortis a lorem. Fusce non finibus nisi, malesuada faucibus libero. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris eget risus ante. In ut enim et ex semper rutrum quis ac mi. Suspen
        </div>
      </div>
      <div class="card">
        <div class="card-header">
          <div class="card-title">
            Название пункта помощи
          </div>
          <div class="card-ico">
            <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
            <path d="M7.41 8.58997L12 13.17L16.59 8.58997L18 9.99997L12 16L6 9.99997L7.41 8.58997Z" fill="#323232"/>
            </svg>
          </div>
        </div>
        <div class="card-text">
          Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque mattis ultricies ipsum. Donec vel consequat ex, id iaculis quam. Proin at massa nunc. Aliquam lacus orci, imperdiet vel sem quis, gravida facilisis mauris. Pellentesque nulla nibh, vehicula vitae sagittis et, lobortis a lorem. Fusce non finibus nisi, malesuada faucibus libero. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris eget risus ante. In ut enim et ex semper rutrum quis ac mi. Suspen
        </div>
      </div>
    </div>
  </section>
  <?php
    include_once "footer.php" ;
  ?>
  <script type="text/javascript" src="scripts/help.js"></script>
</body>
</html>
