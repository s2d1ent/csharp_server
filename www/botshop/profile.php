<!DOCTYPE html>
<html lang="ru">
<head>
    <?php
      include_once "head.php" ;
    ?>
    <link rel="stylesheet" type="text/css" href="/style/profile.css">
</head>
<body>
  <section class="section__profile">
    <div class="profile">
      <div class="profile__title">
        <div class="profile__title-name">
          ?php $user.name ?>
        </div>
        <div class="profile__title-email">
          ?php $user.email ?>
        </div>
      </div>
      <div class="profile-blocks">
        <div class="profile__purchase">
          <div class="profile__purchase-header">
            <a class="profile__purchase-title" href="/profile/history.php">
              Мои покупки
            </a>
          </div>
          <div class="profile__purchase-products">
            <a href="/" class="product">
              <div class="product-img" style="background-image:url('/src/eve.jpg');"></div>
              <div class="product-name">?php $product.name ?></div>
              <div class="product-price">1 200 ₽</div>
            </a>
            <a href="/" class="product">
              <div class="product-img" style="background-image:url('/src/eve.jpg');"></div>
              <div class="product-name">?php $product.name ?></div>
              <div class="product-price">1 200 ₽</div>
            </a>
            <a href="/" class="product">
              <div class="product-img" style="background-image:url('/src/eve.jpg');"></div>
              <div class="product-name">?php $product.name ?></div>
              <div class="product-price">1 200 ₽</div>
            </a>
            <a href="/" class="product">
              <div class="product-img" style="background-image:url('/src/eve.jpg');"></div>
              <div class="product-name">?php $product.name ?></div>
              <div class="product-price">1 200 ₽</div>
            </a>
          </div>
          <div class="profile__purchase-footer">
            <a href="/profile/history.php">Узнать больше</a>
          </div>
        </div>
        <div class="profile__basket">
          <div class="profile__basket-header">
            <a class="profile__basket-title" href="/profile/basket.php">
              Корзина
            </a>
          </div>
          <div class="profile__basket-products">
            <a href="/" class="product">
              <div class="product-img" style="background-image:url('/src/eve.jpg');"></div>
              <div class="product-name">?php $product.name ?></div>
              <div class="product-price">1 200 ₽</div>
            </a>
            <a href="/" class="product">
              <div class="product-img" style="background-image:url('/src/eve.jpg');"></div>
              <div class="product-name">?php $product.name ?></div>
              <div class="product-price">1 200 ₽</div>
            </a>
            <a href="/" class="product">
              <div class="product-img" style="background-image:url('/src/eve.jpg');"></div>
              <div class="product-name">?php $product.name ?></div>
              <div class="product-price">1 200 ₽</div>
            </a>
            <a href="/" class="product">
              <div class="product-img" style="background-image:url('/src/eve.jpg');"></div>
              <div class="product-name">?php $product.name ?></div>
              <div class="product-price">1 200 ₽</div>
            </a>
          </div>
          <a href="/" class="profile__basket-footer">
              <div class="basket-price">1 200 ₽</div>
          </a>
        </div>
      </div>

      <div class="profile-menu">
        <a href="/catalog" class="profile__menu-tile">
          <div class="profile__menu-svg">
            <svg width="58" height="58" viewBox="0 0 58 58" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M44.37 21.75L45.82 29H12.18L13.63 21.75H44.37ZM48.3333 9.66663H9.66667V14.5H48.3333V9.66663ZM48.3333 16.9166H9.66667L7.25 29V33.8333H9.66667V48.3333H33.8333V33.8333H43.5V48.3333H48.3333V33.8333H50.75V29L48.3333 16.9166ZM14.5 43.5V33.8333H29V43.5H14.5Z" fill="#323232"/>
            </svg>
          </div>
        </a>
        <a href="/profile/settings.php" class="profile__menu-tile">
          <div class="profile__menu-svg">
            <svg width="58" height="58" viewBox="0 0 58 58" fill="none" xmlns="http://www.w3.org/2000/svg">
                <path d="M46.9557 31.3684C47.0524 30.595 47.1249 29.8217 47.1249 29C47.1249 28.1784 47.0524 27.405 46.9557 26.6317L52.0549 22.6442C52.514 22.2817 52.6349 21.6292 52.3449 21.0975L47.5115 12.7359C47.294 12.3492 46.8832 12.1317 46.4482 12.1317C46.3032 12.1317 46.1582 12.1559 46.0374 12.2042L40.0199 14.6209C38.7632 13.6542 37.4099 12.8567 35.9357 12.2525L35.0174 5.84837C34.9449 5.26837 34.4374 4.83337 33.8332 4.83337H24.1665C23.5624 4.83337 23.0549 5.26837 22.9824 5.84837L22.064 12.2525C20.5899 12.8567 19.2365 13.6784 17.9799 14.6209L11.9624 12.2042C11.8174 12.1559 11.6724 12.1317 11.5274 12.1317C11.1165 12.1317 10.7057 12.3492 10.4882 12.7359L5.65486 21.0975C5.34069 21.6292 5.48569 22.2817 5.94486 22.6442L11.044 26.6317C10.9474 27.405 10.8749 28.2025 10.8749 29C10.8749 29.7975 10.9474 30.595 11.044 31.3684L5.94486 35.3559C5.48569 35.7184 5.36486 36.3709 5.65486 36.9025L10.4882 45.2642C10.7057 45.6509 11.1165 45.8684 11.5515 45.8684C11.6965 45.8684 11.8415 45.8442 11.9624 45.7959L17.9799 43.3792C19.2365 44.3459 20.5899 45.1434 22.064 45.7475L22.9824 52.1517C23.0549 52.7317 23.5624 53.1667 24.1665 53.1667H33.8332C34.4374 53.1667 34.9449 52.7317 35.0174 52.1517L35.9357 45.7475C37.4099 45.1434 38.7632 44.3217 40.0199 43.3792L46.0374 45.7959C46.1824 45.8442 46.3274 45.8684 46.4724 45.8684C46.8832 45.8684 47.294 45.6509 47.5115 45.2642L52.3449 36.9025C52.6349 36.3709 52.514 35.7184 52.0549 35.3559L46.9557 31.3684ZM42.1707 27.2359C42.2674 27.985 42.2915 28.4925 42.2915 29C42.2915 29.5075 42.2432 30.0392 42.1707 30.7642L41.8324 33.495L43.9832 35.1867L46.5932 37.2167L44.9015 40.1409L41.8324 38.9084L39.319 37.8934L37.144 39.5367C36.1049 40.31 35.114 40.89 34.1232 41.3009L31.5615 42.34L31.1749 45.0709L30.6915 48.3334H27.3082L26.849 45.0709L26.4624 42.34L23.9007 41.3009C22.8615 40.8659 21.8949 40.31 20.9282 39.585L18.729 37.8934L16.1674 38.9325L13.0982 40.165L11.4065 37.2409L14.0165 35.2109L16.1674 33.5192L15.829 30.7884C15.7565 30.0392 15.7082 29.4834 15.7082 29C15.7082 28.5167 15.7565 27.9609 15.829 27.2359L16.1674 24.505L14.0165 22.8134L11.4065 20.7834L13.0982 17.8592L16.1674 19.0917L18.6807 20.1067L20.8557 18.4634C21.8949 17.69 22.8857 17.11 23.8765 16.6992L26.4382 15.66L26.8249 12.9292L27.3082 9.66671H30.6674L31.1265 12.9292L31.5132 15.66L34.0749 16.6992C35.114 17.1342 36.0807 17.69 37.0474 18.415L39.2465 20.1067L41.8082 19.0675L44.8774 17.835L46.569 20.7592L43.9832 22.8134L41.8324 24.505L42.1707 27.2359ZM28.9999 19.3334C23.659 19.3334 19.3332 23.6592 19.3332 29C19.3332 34.3409 23.659 38.6667 28.9999 38.6667C34.3407 38.6667 38.6665 34.3409 38.6665 29C38.6665 23.6592 34.3407 19.3334 28.9999 19.3334ZM28.9999 33.8334C26.3415 33.8334 24.1665 31.6584 24.1665 29C24.1665 26.3417 26.3415 24.1667 28.9999 24.1667C31.6582 24.1667 33.8332 26.3417 33.8332 29C33.8332 31.6584 31.6582 33.8334 28.9999 33.8334Z" fill="#1D1F22"/>
            </svg>

          </div>
        </a>
        <a href="/help" class="profile__menu-tile">
          <div class="profile__menu-svg">
            <svg width="58" height="58" viewBox="0 0 58 58" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M45.9167 33.8333V43.5H41.0833V33.8333H45.9167ZM16.9167 33.8333V43.5H14.5C13.1708 43.5 12.0833 42.4125 12.0833 41.0833V33.8333H16.9167ZM29 2.41663C16.9892 2.41663 7.25 12.1558 7.25 24.1666V41.0833C7.25 45.095 10.4883 48.3333 14.5 48.3333H21.75V29H12.0833V24.1666C12.0833 14.8141 19.6475 7.24996 29 7.24996C38.3525 7.24996 45.9167 14.8141 45.9167 24.1666V29H36.25V48.3333H45.9167V50.75H29V55.5833H43.5C47.5117 55.5833 50.75 52.345 50.75 48.3333V24.1666C50.75 12.1558 41.0108 2.41663 29 2.41663Z" fill="#323232"/>
            </svg>

          </div>
        </a>
        <div id="exit" class="profile__menu-tile" style="cursor: pointer;">
          <div class="profile__menu-svg">
            <svg width="48" height="48" viewBox="0 0 48 48" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M20.18 31.18L23 34L33 24L23 14L20.18 16.82L25.34 22H6V26H25.34L20.18 31.18ZM38 6H10C7.78 6 6 7.8 6 10V18H10V10H38V38H10V30H6V38C6 40.2 7.78 42 10 42H38C40.2 42 42 40.2 42 38V10C42 7.8 40.2 6 38 6Z" fill="#D84452"/>
            </svg>
          </div>
        </div>
      </div>
    </div>
  </section>
  <?php
      include_once "footer.php" ;
  ?>
</body>
</html>
