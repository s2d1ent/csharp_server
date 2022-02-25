<?php
  $catalog_count = 5;
 ?>
<!DOCTYPE html>
<html lang="ru">
<head>
    <?php
      include_once "head.php" ;
    ?>
    <link rel="stylesheet" href="/style/catalog.css">
</head>
<body>
  <?php
    include_once "header.php" ;
  ?>
  <section class="section__catalog">
    <div class="catalog">
      <div class="catalog-text">
        <div class="catalog-title">
          Каталог товаров
        </div>
        <div class="catalog-subtitle">
          <div class="catalog-count">Всего товаров: <?php echo $catalog_count ?></div>
          <div class="catalog-sort">
            Сортировка
            <select class="catalog__sort-select" name="catalog_sort">
              <option value="catalog__sort-name">По имени</option>
              <option value="catalog__sort-upprice">По возрастанию цены</option>
              <option value="catalog__sort-downprice">По снижению цены</option>
              <option value="catalog__sort-popular">По популярности</option>
            </select>
          </div>
        </div>
      </div>
      <div class="catalog-cards">
        <div class="cards-catalog">
          <a href="catalog/#" class="card" style="background-image:url('/src/eve.jpg');">
            <div class="card-title">Бот Eve Online</div>
            <div class="card-prices">
              <div class="card-sale"><span>20%</span></div>
              <div class="card-price">1 200 ₽</div>
            </div>
          </a>
          <a href="catalog/#" class="card" style="background-image:url('/src/nja.jpg');">
            <div class="card-title">Подписка Бот Eve Online</div>
            <div class="card-prices">
              <div class="card-sale" style="display:none;"><span>20%</span></div>
              <div class="card-price">1 200 ₽</div>
            </div>
          </a>
          <a href="catalog/#" class="card" style="background-image:url('/src/eve.jpg');">
            <div class="card-title">Бот Eve Online</div>
            <div class="card-prices">
              <div class="card-sale"><span>20%</span></div>
              <div class="card-price">1 200 ₽</div>
            </div>
          </a>
          <a href="catalog/#" class="card" style="background-image:url('/src/nja.jpg');">
            <div class="card-title">Подписка Бот Eve Online</div>
            <div class="card-prices">
              <div class="card-sale" style="display:none;"><span>20%</span></div>
              <div class="card-price">1 200 ₽</div>
            </div>
          </a>
          <a href="catalog/#" class="card" style="background-image:url('/src/eve.jpg');">
            <div class="card-title">Бот Eve Online</div>
            <div class="card-prices">
              <div class="card-sale"><span>20%</span></div>
              <div class="card-price">1 200 ₽</div>
            </div>
          </a>
          <a href="catalog/#" class="card" style="background-image:url('/src/nja.jpg');">
            <div class="card-title">Подписка Бот Eve Online</div>
            <div class="card-prices">
              <div class="card-sale" style="display:none;"><span>20%</span></div>
              <div class="card-price">1 200 ₽</div>
            </div>
          </a>
          <a href="catalog/#" class="card" style="background-image:url('/src/eve.jpg');">
            <div class="card-title">Бот Eve Online</div>
            <div class="card-prices">
              <div class="card-sale"><span>20%</span></div>
              <div class="card-price">1 200 ₽</div>
            </div>
          </a>
          <a href="catalog/#" class="card" style="background-image:url('/src/nja.jpg');">
            <div class="card-title">Подписка Бот Eve Online</div>
            <div class="card-prices">
              <div class="card-sale" style="display:none;"><span>20%</span></div>
              <div class="card-price">1 200 ₽</div>
            </div>
          </a>
          <a href="catalog/#" class="card" style="background-image:url('/src/eve.jpg');">
            <div class="card-title">Бот Eve Online</div>
            <div class="card-prices">
              <div class="card-sale"><span>20%</span></div>
              <div class="card-price">1 200 ₽</div>
            </div>
          </a>
          <a href="catalog/#" class="card" style="background-image:url('/src/nja.jpg');">
            <div class="card-title">Подписка Бот Eve Online</div>
            <div class="card-prices">
              <div class="card-sale" style="display:none;"><span>20%</span></div>
              <div class="card-price">1 200 ₽</div>
            </div>
          </a>
        </div>
        <div class="cards-filter">
          <div class="filter-games">

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
