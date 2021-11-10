<!DOCTYPE html>
<html lang="ru">
<head>
    <?php
      include_once "head.php" ;
    ?>
    <link rel="stylesheet" href="/style/index.css">
</head>
<body>
  <?php
    include_once "header.php" ;
  ?>
  <section class="section__minicatalog">
    <div class="minicatalog">
      <div class="minicatalog__field">
        <div class="minicatalog-title">
          <div class="minicatalog-name">
            Боты
          </div>
          <div class="minicatalog-count">
            Всего товаров:
          </div>
        </div>
        <div class="minicatalog-cards">
          <a href="/" class="minicatalog-card" style="background-image:url('src/wow.jpg');">
            <div class="card-title">
              World of Warcraft
            </div>
          </a>
          <a href="/" class="minicatalog-card" style="background-image:url('src/eve.jpg');">
            <div class="card-title">
              Eve Online
            </div>
          </a>
          <a href="/" class="minicatalog-card" style="background-image:url('src/bd.jpg');">
            <div class="card-title">
              Black Desert
            </div>
          </a>
          <a href="catalog" class="minicatalog-card" style="background-image:url('src/bots.png');">
            <div class="card-title">
              Смотреть весь каталог
            </div>
          </a>

        </div>
      </div>
      <div class="minicatalog__field">
        <div class="minicatalog-title">
          <div class="minicatalog-name">
            Подписки
          </div>
          <div class="minicatalog-count">
            Всего товаров:
          </div>
        </div>
        <div class="minicatalog-cards">
          <a href="/" class="minicatalog-card" style="background-image:url('src/wow.jpg');">
            <div class="card-title">
              World of Warcraft
            </div>
          </a>
          <a href="/" class="minicatalog-card" style="background-image:url('src/eve.jpg');">
            <div class="card-title">
              Eve Online
            </div>
          </a>
          <a href="/" class="minicatalog-card" style="background-image:url('src/bd.jpg');">
            <div class="card-title">
              Black Desert
            </div>
          </a>
          <a href="catalog" class="minicatalog-card" style="background-image:url('src/bots.png');">
            <div class="card-title">
              Смотреть весь каталог
            </div>
          </a>

        </div>
      </div>
    </div>
  </section>
  <?php
    include_once "slider.php" ;
  ?>
  <?php
    include_once "footer.php" ;
  ?>
</body>
</html>
