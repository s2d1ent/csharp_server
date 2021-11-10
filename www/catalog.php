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
    <div class="catalog-title">
      Каталог товаров
    </div>
    <div class="catalog-count_and_sort">
      <div class="catalog-count">
        Всего товаров: <?php echo $catalog_count; ?>
      </div>
      <div class="catalog-sort">
        <select class="catalog__sort-select" name="catalog_sort">
          <option value="catalog__sort-name">По имени</option>
          <option value="catalog__sort-upprice">По возрастанию цены</option>
          <option value="catalog__sort-downprice">По снижению цены</option>
          <option value="catalog__sort-popular">По популярности</option>
        </select>
      </div>
    </div>
    <div class="catalog-cards">
      <div class="cards-catalog">
        <div class="card" style="background:url('/src/eve.jpg');">
          
        </div>
      </div>
      <div class="cards-filter">

      </div>
    </div>
  </div>
</section>
  <?php
    include_once "footer.php" ;
  ?>
</body>
</html>
